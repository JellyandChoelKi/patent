using K2GGTT.Models;
using K2GGTT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using K2GGTT.Data;
using Newtonsoft.Json;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using K2GGTT.Utils;
using Newtonsoft.Json.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.IO.Compression;
using System.Threading;
namespace K2GGTT.Controllers
{
	

	public static class XMLExtension
	{
		public static string ReplaceAt(this string str, int index, int length, string replace)
		{
			return str.Remove(index, Math.Min(length, str.Length - index)).Insert(index, replace);
		}

		public static string UnescapeXMLValue(this string str)
		{
			if (str == null) throw new ArgumentNullException("xmlString");
			return str
				.Replace("&apos;", "'")
				.Replace("&quot;", "\"")
				.Replace("&gt;", ">")
				.Replace("&lt;", "<")
				.Replace("&amp;", "&")
				.Replace("&deg;", "")
				.Replace("&le;", "")
				.Replace("&lsqb;", "");
		}
	}

	public class KrController : Controller
	{
		private readonly ILogger<KrController> _logger;
		private readonly DBContext _context;

		//맥주소 입력해주세요.
		public static string MAC_address = "80-FA-5B-68-EB-43";
		//발급받은 client_id를 입력해주세요.
		public static string clientID = "0ade3f2ef692f608addbaeb263fd3356809bdb01007fd09ccfcfcfb76b0895dd";
		//API Gateway로부터 발급받은 인증키를 입력해주세요.
		public static string key = "128e05eb22ca4396adbdad82dc369f96";

		//refresh Token을 입력해주세요.(Access Token 재발급시 입력)
		public static string refreshToken = "Your RefreshToken";
		//accessToken을 입력해주세요(데이터요청시 이용)
		public static string accessToken = "Your AccessToken";
		//iv의 값은 변하지 않습니다.
		private static string iv = "jvHJ1EFA0IXBrxxz";

		public string SHA256Hash(string password)
		{
			SHA256 sha = new SHA256Managed();
			byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(password));
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte b in hash)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return stringBuilder.ToString();
		}

		public KrController(ILogger<KrController> logger, DBContext DBContext)
		{
			_logger = logger;
			_context = DBContext;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return Content(@"<script type='text/javascript'>alert('정상적으로 로그아웃 되었습니다.');location.href='/Kr/Index';</script>", "text/html", System.Text.Encoding.UTF8);
		}

		public IActionResult Action(string keywords)
		{
			ViewBag.Keywords = keywords;
			return View();
		}

		public IActionResult Result(string keywords)
		{
			ViewBag.Keywords = keywords;
			return View();
		}

		public IActionResult Report(string type, string id)
		{
			var tokenResponse = createToken();

			string query = HttpUtility.UrlEncode("{\"CN\":\"" + id + "\"}");
			string target_URL = "https://apigateway.kisti.re.kr/openapicall.do?" +
			"client_id=" + clientID + "&token=" + accessToken + "&version=1.0" + "&action=search" +
			"&target=ARTI" + "&searchQuery=" + query;

			string response = getResponse(target_URL);
			Console.WriteLine(response);

			XmlDocument xml = new XmlDocument();
			xml.LoadXml(response);

			var statusCode = xml["MetaData"]["resultSummary"]["statusCode"].InnerText;

			XmlNodeList nodeList = xml["MetaData"]["recordList"].GetElementsByTagName("record");
			foreach (XmlNode record in nodeList)
			{
				foreach (XmlNode item in record.ChildNodes)
				{
					if (item.Attributes["metaCode"].InnerText.Equals("Title"))
					{
						ViewBag.Title = item.InnerText;
					}
					else if (item.Attributes["metaCode"].InnerText.Equals("Abstract"))
					{
						ViewBag.Abstract = item.InnerText;
					}
					else if (item.Attributes["metaCode"].InnerText.Equals("Author"))
					{
						ViewBag.Author = item.InnerText;
					}
					else if (item.Attributes["metaCode"].InnerText.Equals("Pubyear"))
					{
						ViewBag.Pubyear = item.InnerText;
					}
				}
			}
			return View();
		}

		// 사용자용(압축파일) 나중에
		/*
		public IActionResult UserPatentDataMigrations(string target_zip_file)
		{
			MongoDBConf db = new MongoDBConf();
			BsonDocument BsonDoc = new BsonDocument();

			string WWWROOTPATH = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "kipris" + Path.DirectorySeparatorChar;
			string zipFilePath = System.IO.Path.Combine(WWWROOTPATH, target_zip_file);  // 같은 폴더에 압축풀고
			string extractionPath = WWWROOTPATH;

			// 압축을 다 풀기도 전에 아래 프로세스를 실행해버림... 스레드 끝나고 돌려주자
			var t = new Thread(() =>
			{
				ZipFile.ExtractToDirectory(zipFilePath, extractionPath);
			});
			t.Start();

			// 자동으로 점유율 끝날때까지 기다려야할듯 2초정도??
			Thread.Sleep(2000);

			System.IO.File.Delete(zipFilePath);

			// 사용자용 웹루트 경로에 타겟폴더 kipris로 만들어줘야함
			string PATH = System.IO.Path.Combine(WWWROOTPATH, target_zip_file);
			List<string> xmlPathList = new List<string>();
			DirectoryInfo di = new DirectoryInfo(PATH);
			if (di.Exists)
			{
				DirectoryInfo[] di_arr = di.GetDirectories("*", SearchOption.AllDirectories);
				foreach (DirectoryInfo xmlPath in di_arr)
				{
					if (xmlPath.FullName.IndexOf("Examined" + Path.DirectorySeparatorChar + "xml") != -1)
					{
						xmlPathList.Add(xmlPath.FullName);
					}
				}
			}

			foreach (var path in xmlPathList)
			{
				string[] folder_arr = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
				foreach (string file in folder_arr)
				{
					try
					{
						string xml_to_str = HttpUtility.HtmlDecode(System.IO.File.ReadAllText(file)).Replace("&", "");
						XDocument doc = XDocument.Parse(xml_to_str);

						// 출원번호
						XElement node = (from n in doc.Descendants("B210") select n).FirstOrDefault();
						string app_no = node.Value;

						try
						{
							// UnExamined에 이미 중복 출원번호 있으면 패스
							var p = db.LoadRecordById<BsonDocument>("kipris", "app_no", app_no);
							if (p != null)
							{
								continue;
							}
						}
						catch
						{
							string to_json_str = JsonConvert.SerializeObject(doc);
							string parse_json_str = to_json_str.ReplaceAt(to_json_str.Length - 1, 0, ",\"app_no\":\"" + app_no + "\"");

							BsonDoc = BsonDocument.Parse(parse_json_str);
							db.InsertRecord("kipris", BsonDoc);

							System.IO.File.Delete(file);
						}
					}
					catch
					{
						try
						{
							XmlDocument doc = new XmlDocument();
							doc.Load(file);

							// 출원번호
							XmlNode node = doc.SelectSingleNode("/kr-patent-document/SDOBI/B200/B210");
							string app_no = node.InnerText;

							try
							{
								// UnExamined에 이미 중복 출원번호 있으면 패스
								var p = db.LoadRecordById<BsonDocument>("kipris", "app_no", app_no);
								if (p != null)
								{
									continue;
								}

							}
							catch
							{
								string to_json_str = JsonConvert.SerializeObject(doc);
								string parse_json_str = to_json_str.ReplaceAt(to_json_str.Length - 1, 0, ",\"app_no\":\"" + app_no + "\"");

								BsonDoc = BsonDocument.Parse(parse_json_str);
								db.InsertRecord("kipris", BsonDoc);

								System.IO.File.Delete(file);
							}
						}
						catch (Exception e)
						{
							JObject rv = new JObject();
							rv.Add("error_path", file);
							rv.Add("error_msg", e.Message);
							string toJson = JsonConvert.SerializeObject(rv);

							BsonDoc = BsonDocument.Parse(toJson);
							db.InsertRecord("kipris_error_log", BsonDoc);
						}
					}
				}
			}

			// 에러파일 이동시키자
			var list = from a in db.LoadRecord<FilePath>("kipris_error_log") select new FilePath { error_path = a.error_path };
			foreach (var p in list.ToList())
			{
				string file_path = p.error_path;
				string[] file_arr = p.error_path.Split(new string[] { "\\" }, StringSplitOptions.None);
				string file = file_arr[file_arr.Length - 1];
				try
				{
					string error_folder = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "wwwroot" + Path.DirectorySeparatorChar + "kipris_error" + Path.DirectorySeparatorChar;
					string target_path = System.IO.Path.Combine(error_folder, file);  // 사용자용 웹루트 경로에 타겟폴더 kipris_error로 만들어줘야함
					FileInfo info = new FileInfo(target_path);
					if (!info.Exists)
					{
						System.IO.File.Copy(file_path, target_path, true);
					}
				}
				catch
				{

				}
			}
			return Ok();
		}
		*/

		public IActionResult Contact()
		{
			return View();
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult LoginProc(string memberid, string password)
		{
			var member = _context.Member.Where(x => x.MemberId == memberid && x.Password == SHA256Hash(password)).FirstOrDefault();
			if (member == null)
			{
				return Content(@"<script type='text/javascript'>alert('로그인 정보가 없습니다.');history.back();</script>", "text/html", System.Text.Encoding.UTF8);
			}
			HttpContext.Session.SetString("Id", member.Id.ToString());
			HttpContext.Session.SetString("MemberId", member.MemberId);
			HttpContext.Session.SetString("MemberName", member.Name);
			return Redirect("/Kr/Index");
		}
		public IActionResult Password()
		{
			return View();
		}

		[HttpPost]
		public IActionResult NewPasswordCreate(string email)
		{
			Random r = new Random();
			const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			string code = new string(Enumerable.Repeat(characters, 6).Select(s => s[r.Next(s.Length)]).ToArray());

			var member = _context.Member.Where(x => x.Email == email).FirstOrDefault();
			if (member == null)
			{
				return Content(@"<script type='text/javascript'>alert('가입된 이메일 정보가 없습니다. 다시 확인 바랍니다.');history.back();</script>", "text/html", System.Text.Encoding.UTF8);
			}
			member.Password = SHA256Hash(code);
			_context.SaveChanges();

			MailMessage mail = new MailMessage();
			mail.From = new MailAddress("no-reply@yellowknife.io", "K2G", System.Text.Encoding.UTF8);
			mail.To.Add(email);
			mail.Subject = "[K2G] 임시 비밀번호가 발급되었습니다.";
			mail.Body = "임시 비밀번호를 전송해 드립니다. 확인 후 회원정보 수정을 통해 비밀번호를 변경하시기 바랍니다. 비밀번호 : " + code;
			mail.IsBodyHtml = true;
			mail.SubjectEncoding = System.Text.Encoding.UTF8;
			mail.BodyEncoding = System.Text.Encoding.UTF8;
			SmtpClient SmtpServer = new SmtpClient("email-smtp.ap-northeast-2.amazonaws.com");
			SmtpServer.Port = 587;
			SmtpServer.Credentials = new System.Net.NetworkCredential("AKIAUEOAKJWYTC4CMF7U", "BJhoD0cuzQMgK0osvQ6r0vvr9P2SweePd3Cm3V5iz9c2");
			SmtpServer.EnableSsl = true;
			SmtpServer.Send(mail);
			mail.Dispose();

			return Content(@"<script type='text/javascript'>alert('입력하신 이메일 주소로 새로운 비밀번호가 전송되었습니다.');location.href='/Kr/Login';</script>", "text/html", System.Text.Encoding.UTF8);
		}

		public IActionResult About()
		{
			return View();
		}

		public IActionResult Register()
		{
			return View();
		}

		public IActionResult SignUp()
		{
			MyInfoViewModel model = new MyInfoViewModel();
			return View(model);
		}

		[HttpPost]
		public IActionResult MemberIdCheck(string memberid)
		{
			string response = "N";
			if (!string.IsNullOrEmpty(memberid))
			{
				var Id = _context.Member.Where(x => x.MemberId == memberid).FirstOrDefault();
				if (Id != null) response = "Y";
			}
			return Ok(JsonConvert.SerializeObject(response));
		}

		[HttpPost]
		public IActionResult EmailCertification(string email)
		{
			Random r = new Random();
			const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			string code = new string(Enumerable.Repeat(characters, 6).Select(s => s[r.Next(s.Length)]).ToArray());

			MailMessage mail = new MailMessage();
			mail.From = new MailAddress("no-reply@yellowknife.io", "K2G", System.Text.Encoding.UTF8);
			mail.To.Add(email);
			mail.Subject = "[K2G] 이메일 인증코드를 보내드립니다.";
			mail.Body = "사용자 인증을 위해 6자리 코드 보내드립니다. 회원가입 화면에 6자리 코드 입력 바랍니다. 인증코드 : " + code;
			mail.IsBodyHtml = true;
			mail.SubjectEncoding = System.Text.Encoding.UTF8;
			mail.BodyEncoding = System.Text.Encoding.UTF8;
			SmtpClient SmtpServer = new SmtpClient("email-smtp.ap-northeast-2.amazonaws.com");
			SmtpServer.Port = 587;
			SmtpServer.Credentials = new System.Net.NetworkCredential("AKIAUEOAKJWYTC4CMF7U", "BJhoD0cuzQMgK0osvQ6r0vvr9P2SweePd3Cm3V5iz9c2");
			SmtpServer.EnableSsl = true;
			SmtpServer.Send(mail);
			mail.Dispose();

			return Ok(JsonConvert.SerializeObject(code));
		}

		[HttpPost]
		public IActionResult SignUpProc(MyInfoViewModel model)
		{
			Member member = new Member();
			member.MemberId = model.MemberId;
			member.Password = SHA256Hash(model.Password);
			member.Gubun = model.Gubun;
			member.Name = model.Name;
			member.CompanyRegistrationNumber = model.CompanyRegistrationNumber;
			member.Representative = model.Representative;
			member.TaxInvoiceOfficer = model.TaxInvoiceOfficer;
			member.Contact = model.Contact;
			member.Zipcode = model.Zipcode;
			member.Addr1 = model.Addr1;
			member.Addr2 = model.Addr2;
			member.Email = model.Email;
			_context.Member.Add(member);
			_context.SaveChanges();

			return Content(@"<script type='text/javascript'>alert('회원가입이 완료되었습니다.');location.href='/Kr/Index';</script>", "text/html", System.Text.Encoding.UTF8);
		}

		public IActionResult MyPage()
		{
			MyInfoViewModel model = new MyInfoViewModel();
			int Id = Convert.ToInt32(HttpContext.Session.GetString("Id"));
			if (Id > 0)
			{
				var member = _context.Member.Where(x => x.Id == Id).FirstOrDefault();
				model.Id = member.Id;
				model.MemberId = member.MemberId;
				model.Name = member.Name;
				model.Gubun = member.Gubun;
				model.CompanyRegistrationNumber = member.CompanyRegistrationNumber;
				model.Representative = member.Representative;
				model.TaxInvoiceOfficer = member.TaxInvoiceOfficer;
				model.Contact = member.Contact;
				model.Zipcode = member.Zipcode;
				model.Addr1 = member.Addr1;
				model.Addr2 = member.Addr2;
				model.Email = member.Email;
			}
			return View(model);
		}

		[HttpPost]
		public IActionResult MyPageProc(MyInfoViewModel model)
		{
			if (model.Id <= 0)
			{
				return Content(@"<script type='text/javascript'>alert('알수없는 이유로 세션이 종료되었습니다. 다시 로그인 바랍니다.');location.href='/Kr/Login';</script>", "text/html", System.Text.Encoding.UTF8);
			}
			var member = _context.Member.Where(x => x.Id == model.Id).FirstOrDefault();
			if (member.Password != SHA256Hash(model.CurrentPassword))
			{
				return Content(@"<script type='text/javascript'>alert('비밀번호가 일치하지 않습니다.');history.back();</script>", "text/html", System.Text.Encoding.UTF8);
			}
			if (!string.IsNullOrEmpty(model.Password))
			{
				member.Password = SHA256Hash(model.Password);
			}
			member.Gubun = model.Gubun;
			member.Name = model.Name;
			member.CompanyRegistrationNumber = model.CompanyRegistrationNumber;
			member.Representative = model.Representative;
			member.TaxInvoiceOfficer = model.TaxInvoiceOfficer;
			member.Contact = model.Contact;
			member.Zipcode = model.Zipcode;
			member.Addr1 = model.Addr1;
			member.Addr2 = model.Addr2;
			_context.SaveChanges();

			return Content(@"<script type='text/javascript'>alert('회원정보 수정이 완료되었습니다.');location.href='/Kr/MyPage';</script>", "text/html", System.Text.Encoding.UTF8);
		}

		/**
		 * 1) 최초 토큰발급 요청인 경우, RefreshToken 값이 만료(2주 기한)되어 신규로 AccessToken, RefreshToken 둘 다 전체 토큰발급이 필요한 경우
		 * 2) API Gateway 신청시 제출한 맥주소 값, 발급받은 클라이어트 ID 값이 필요함
		 * 3) 정상적으로 토큰발급이 완료되면, AccessToken, RefreshToken 값을 저장한 이 후에 이 값을 사용해야 함
		 */
		public static string createToken()
		{
			string date = DateTime.Now.ToString("yyyyMMddHHmmss");

			var json = new JObject();
			json.Add("datetime", date);
			json.Add("mac_address", MAC_address);

			string encrypted_txt = HttpUtility.UrlEncode(encrypt(json.ToString(), key));

			string target_URL = "https://apigateway.kisti.re.kr/tokenrequest.do?accounts=" + encrypted_txt +
			"&client_id=" + clientID;


			JObject responseJson = JObject.Parse(getResponse(target_URL));
			refreshToken = responseJson["refresh_token"].ToString();
			accessToken = responseJson["access_token"].ToString();
			Console.WriteLine("access_token : " + accessToken);
			Console.WriteLine("refresh_token : " + refreshToken);
			return responseJson.ToString();
		}

		public static string getResponse(string target_URL)
		{
			string responseText = string.Empty;

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(target_URL);
			request.Method = "GET";

			try
			{
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					HttpStatusCode statusCode = response.StatusCode;
					Console.WriteLine(statusCode);

					Stream respStream = response.GetResponseStream();
					using (StreamReader sr = new StreamReader(respStream))
					{
						responseText = sr.ReadToEnd();
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			return responseText;
		}

		public static string encrypt(string plainText, string key)
		{
			{
				UTF8Encoding ue = new UTF8Encoding();

				using (Aes aesAlg = Aes.Create())
				{
					aesAlg.Padding = PaddingMode.PKCS7;
					aesAlg.Mode = CipherMode.CBC;
					aesAlg.Key = ue.GetBytes(key);
					aesAlg.IV = ue.GetBytes(iv);

					byte[] message = ue.GetBytes(plainText.Replace("\r\n", "").Replace(" ", ""));
					byte[] enc;

					// Create an encryptor to perform the stream transform.
					ICryptoTransform encryptor = aesAlg.CreateEncryptor();

					using (MemoryStream msEncrypt = new MemoryStream())
					{
						using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
						{
							csEncrypt.Write(message, 0, message.Length);
							
						}

						enc = msEncrypt.ToArray();
					}
					return Convert.ToBase64String(enc, 0, enc.Length).Replace("/", "_").Replace("+", "-");
				}
			}
		}
	}
}
