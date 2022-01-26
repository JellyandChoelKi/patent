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
	public class EnController : Controller
    {
        private readonly ILogger<EnController> _logger;
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

		public EnController(ILogger<EnController> logger, DBContext DBContext)
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
			return Content(@"<script type='text/javascript'>alert('You have logged out successfully.');location.href='/Kr/Index';</script>", "text/html", System.Text.Encoding.UTF8);
		}

		public IActionResult Result(string keywords)
		{
			ViewBag.PatentKeyword = keywords.Replace(' ', '*');
			ViewBag.ArticleKeyword = keywords;

			return View();
		}

		public IActionResult PatentReport(string id)
		{
			return View();
		}

		public IActionResult ArticleReport(string id)
		{
			ArticleViewModel model = new ArticleViewModel()
			{
				TitleList = new List<string>(),
				ArticleIdList = new List<string>(),
				AbstractList = new List<string>(),
				AuthorList = new List<string>(),
				PubyearList = new List<string>(),
				JournalNameList = new List<string>(),
				VolNo1List = new List<string>(),
				VolNo2List = new List<string>(),
				PageInfoList = new List<string>(),
				KeywordList = new List<string>(),
				ContentURLList = new List<string>()
			};
			var tokenResponse = createToken();
			var id_Arr = id.Split('|');

			foreach (var ids in id_Arr)
			{
				string query = HttpUtility.UrlEncode("{\"CN\":\"" + ids + "\"}");
				string target_URL = "https://apigateway.kisti.re.kr/openapicall.do?" +
				"client_id=" + clientID + "&token=" + accessToken + "&version=1.0" + "&action=search" +
				"&target=ARTI" + "&searchQuery=" + query;

				string response = getResponse(target_URL);
				Console.WriteLine(response);

				XmlDocument xml = new XmlDocument();
				xml.LoadXml(response);

				XmlNodeList nodeList = xml["MetaData"]["recordList"].GetElementsByTagName("record");
				foreach (XmlNode record in nodeList)
				{
					foreach (XmlNode item in record.ChildNodes)
					{
						if (item.Attributes["metaCode"].InnerText.Equals("Title"))
						{
							model.TitleList.Add(item.InnerText);
						}
						else if (item.Attributes["metaCode"].InnerText.Equals("ArticleId"))
						{
							model.ArticleIdList.Add(item.InnerText);
						}
						else if (item.Attributes["metaCode"].InnerText.Equals("Abstract"))
						{
							model.AbstractList.Add(item.InnerText);
						}
						else if (item.Attributes["metaCode"].InnerText.Equals("Author"))
						{
							model.AuthorList.Add(item.InnerText);
						}
						else if (item.Attributes["metaCode"].InnerText.Equals("Pubyear"))
						{
							model.PubyearList.Add(item.InnerText);
						}
						else if (item.Attributes["metaCode"].InnerText.Equals("JournalName"))
						{
							model.JournalNameList.Add(item.InnerText);
						}
						else if (item.Attributes["metaCode"].InnerText.Equals("VolNo1"))
						{
							model.VolNo1List.Add(item.InnerText);
						}
						else if (item.Attributes["metaCode"].InnerText.Equals("VolNo2"))
						{
							model.VolNo2List.Add(item.InnerText);
						}
						else if (item.Attributes["metaCode"].InnerText.Equals("PageInfo"))
						{
							model.PageInfoList.Add(item.InnerText);
						}
						else if (item.Attributes["metaCode"].InnerText.Equals("Keyword"))
						{
							model.KeywordList.Add(item.InnerText);
						}
						else if (item.Attributes["metaCode"].InnerText.Equals("ContentURL"))
						{
							model.ContentURLList.Add(item.InnerText);
						}
					}
				}
			}

			return View(model);
		}



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
				return Content(@"<script type='text/javascript'>alert('No Log-in Information.');history.back();</script>", "text/html", System.Text.Encoding.UTF8);
			}
			HttpContext.Session.SetString("Id", member.Id.ToString());
			HttpContext.Session.SetString("MemberId", member.MemberId);
			HttpContext.Session.SetString("MemberName", member.Name);
			return Redirect("/En/Index");
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
				return Content(@"<script type='text/javascript'>alert('No registered email address. Please try again.');history.back();</script>", "text/html", System.Text.Encoding.UTF8);
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

			return Content(@"<script type='text/javascript'>alert('A new password has been sent to your email address.');location.href='/En/Login';</script>", "text/html", System.Text.Encoding.UTF8);
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
			mail.Subject = "[K2G] We will send you an email verification code.";
			mail.Body = "We will send you a 6-digit code for verification. Please enter the 6-digit code on the sign-up screen. Verification code : " + code;
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

			return Content(@"<script type='text/javascript'>alert('You have signed up successfully.');location.href='/En/Index';</script>", "text/html", System.Text.Encoding.UTF8);
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
				return Content(@"<script type='text/javascript'>alert('The session was terminated for an unknown reason. Please try again.');location.href='/En/Login';</script>", "text/html", System.Text.Encoding.UTF8);
			}
			var member = _context.Member.Where(x => x.Id == model.Id).FirstOrDefault();
			if (member.Password != SHA256Hash(model.CurrentPassword))
			{
				return Content(@"<script type='text/javascript'>alert('Password is not correct.');history.back();</script>", "text/html", System.Text.Encoding.UTF8);
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

			return Content(@"<script type='text/javascript'>alert('You have completed editing your information.');location.href='/En/MyPage';</script>", "text/html", System.Text.Encoding.UTF8);
		}
	}
}
