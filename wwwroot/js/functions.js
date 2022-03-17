let currentPage = 1;
const pagePosts = 30;

function loadToc(legendName, type, lang) {
	$("#table_wrap").show();
	$("#load_all").hide();
	$("#load_all > #patent_all").html('');
	$("#load_all > #article_all").html('');
	var currentPage = 1;
	var dataFrom = data[type][legendName];
	var pageData = '';
	var rowData = '';
	var tableTitle = '';
	let buttons = '';

	if (type == 'patent') {
		if (lang == 'en') {
			var tableTitle = "Patent";
			var tableHead = "<tr><th class='no'><input type='checkbox' name='checkboxAll' value='selectall' onclick='selectAll(this)' /></th><th class='no'>No.</th><th class='title'>Title</th><th>Simularity</th><th class='year'>Date</th><th class='report'></th></tr>";
		} else {
			var tableTitle = "특허";
			var tableHead = "<tr><th class='no'><input type='checkbox' name='checkboxAll' value='selectall' onclick='selectAll(this)' /></th><th class='no'>순번</th><th class='title'>발명제목</th><th>유사도</th><th class='year'>출원일</th><th class='report'></th></tr>";
		}
		var subject = "inventionname";
		var author = "inventors";
		var time = "applicationdate";
		var id = "applicationno";
	} else if (type == 'article') {
		if (lang == 'en') {
			var tableTitle = "Articles";
			var tableHead = "<tr><th class='no'><input type='checkbox' name='checkboxAll' value='selectall' onclick='selectAll(this)' /></th><th class='no'>No.</th><th class='title'>Title</th><th>Simularity</th><th class='year'>Year</th><th class='report'></th></tr>";
		} else {
			var tableTitle = "논문";
			var tableHead = "<tr><th class='no'><input type='checkbox' name='checkboxAll' value='selectall' onclick='selectAll(this)' /></th><th class='no'>순번</th><th class='title'>제목</th><th>유사도</th><th class='year'>연도</th><th class='report'></th></tr>";
		}
		var subject = "title";
		var author = "author";
		var time = "pubyear";
		var id = "articleid";
	}

	$('#table_wrap > table').html('');
	var dataCount = dataFrom["coordinate"].length;
	var pageCount = parseInt(dataCount / pagePosts) + 1;
	console.log(dataCount);
	if (dataCount > 0) {

		if (lang == 'en') {
			buttons = "<button class='pad10' onclick='ExcelDownload(\"" + type + "\", \"" + lang + "\")'>Excel Download</button><button class='pad10' onClick='downAll(\"" + type + "\", \"" + lang + "\")'>PDF Download</button>";
		} else {
			buttons = "<button class='pad10' onclick='ExcelDownload(\"" + type + "\", \"" + lang + "\")'>엑셀 다운로드</button><button class='pad10' onClick='downAll(\"" + type + "\", \"" + lang + "\")'>PDF 다운로드</button>";
		}

		$('#table_title').html(tableTitle);
		$('#btn_wrap').html(buttons);
		$('#table_wrap > table').html(tableHead);
		for (i = 0; i < dataCount; i++) {
			let simularity = (dataFrom["coordinate"][i][0] * 100).toFixed(1);
			$('#table_wrap > table').append('<tr class="rows" id="row' + (i + 1) + '"><td><input onChange="countCheck(' + i + ', \'' + lang + '\')" type="checkbox" name="checkbox" id="chk' + (i + 1) + '" data-id="' + dataFrom[id][i] + '"></td><td>' + (i + 1) + '</td><td><a onClick=popupReport(\"' + type + '\",\"' + dataFrom[id][i] + '\") class="bold">' + dataFrom[subject][i] + '</a></td><td>' + (simularity) + '</td><td>' + dataFrom[time][i] + '</td><td class="tc"><a onClick=popupReport(\"' + type + '\",\"' + dataFrom[id][i] + '\") class="mhide"><img src="/img/seo.png" style="width:50px;"></a></td></tr>');
		}
	} else if (dataCount == 0) {
		$('#table_wrap > table').html('<tr><th>No posts here.</th></tr>');
	}
	for (i = 0; i < pageCount; i++) {
		pageData += '<span onClick=movingPage(' + (i + 1) + ',\'' + type + '\') class="pad10 paging page' + (i + 1) + '">' + (i + 1) + '</span>';
	}
	$('#pagination').html(pageData);
	pagination(currentPage, type);
}

function loadAll() {
	$("#table_wrap").hide();
	$("#load_all").show();
	$("#table_wrap > table").html('');

	// 특허 TOC
	let patentCount = 0;
	let patentData = data["patent"];
	$("#load_all > #patent_all").html('<tr><th class="no">순번</th><th class="title">발명제목</th><th class="similarity">유사도</th><th class="year">출원일</th><th class="report"></th></tr>');
	for (i = 0; i < legends["patent"].length; i++) { // 특허 카테고리 수 만큼
		let legendNow = legends["patent"][i];
		if (patentData[legendNow]["inventionname"].length > 0) {
			for (n = 0; n < patentData[legendNow]["inventionname"].length; n++) { // 발명 제목 항목 갯수만큼 반복
				patentCount++;
				let simularity = (patentData[legendNow]["coordinate"][n][0] * 100).toFixed(1);
				$('#load_all > #patent_all').append('<tr class="rows" id="patent_row' + patentCount + '"><td class="no">' + patentCount + '</td><td class="title bold">' + patentData[legendNow]["inventionname"][n] + '</td><td class="simularity">' + simularity + '</td><td class="year">' + patentData[legendNow]["applicationdate"][n] + '</td><td class="report"><a onClick=popupReport(\"patent\",\"' + patentData[legendNow]["applicationno"][n] + '\") class="mhide"><img src="/img/seo.png" style="width:50px;"></a></td></tr>');
			}
		}
	}

	// 논문 TOC
	let articleCount = 0;
	let articleData = data["article"];
	$("#load_all > #article_all").html('<tr><th class="no">순번</th><th class="title">제목</th><th class="similarity">유사도</th><th class="year">연도</th><th class="report"></th></tr>');
	for (i = 0; i < legends["article"].length; i++) { // 특허 카테고리 수 만큼
		let legendNow = legends["article"][i];
		if (articleData[legendNow]["title"].length > 0) {
			for (n = 0; n < articleData[legendNow]["title"].length; n++) { // 발명 제목 항목 갯수만큼 반복
				articleCount++;
				let simularity = (articleData[legendNow]["coordinate"][n][0] * 100).toFixed(1);
				$('#load_all > #article_all').append('<tr class="rows" id="patent_row' + articleCount + '"><td class="no">' + articleCount + '</td><td class="title bold">' + articleData[legendNow]["title"][n] + '</td><td class="simularity">' + simularity + '</td><td class="year">' + articleData[legendNow]["pubyear"][n] + '</td><td class="report"><a onClick=popupReport(\"patent\",\"' + articleData[legendNow]["articleid"][n] + '\") class="mhide"><img src="/img/seo.png" style="width:50px;"></a></td></tr>');
			}
		}
	}
	$("#patent_all_title").show();
	$("#article_all_title").show();
}
function selectAll(selectAll) {
	const checkboxes = document.getElementsByName('checkbox');
	checkboxes.forEach((checkbox) => {
		checkbox.checked = selectAll.checked;
	})
}
function ExcelDownload(type, lang) {
	let arr = [];
	$('input[name="checkbox"]:checked').each(function () {
		arr.push($(this).data('id'));
	});
	if (arr.length <= 0) {
		if (lang == 'en') {
			alert("Please check the items you want to download as Excel.");
		} else {
			alert("엑셀 다운로드 하실 항목을 체크해주세요.");
		}
		return false;
	}
	location.href = "/Kr/" + type + "ExcelDownload?id=" + arr.join('|');
}
function downAll(type, lang) {
	let arr = [];
	$('input[name="checkbox"]:checked').each(function () {
		arr.push($(this).data('id'));
	});
	if (arr.length <= 0) {
		if (lang == 'en') {
			alert("Please check the items you want to download as PDF.");
		} else {
			alert("PDF 다운로드 하실 항목을 체크해주세요.");
		}
		return false;
	}
	//setCookie("pdfDownload", "false");
	//blockLoadingBar();
	if (type == "patent") {
		console.log(arr.join('|'));
		location.href = "/Kr/TocAllDataPDFDownload?id=" + arr.join('|');
	} else {
		location.href = "/Kr/ArticleTocAllDataPDFDownload?id=" + arr.join('|');
	}
}
function setCookie(name, value, expiredays) {
	var todayDate = new Date();
	todayDate.setDate(todayDate.getDate() + expiredays);
	document.cookie = name + "=" + escape(value) + "; path=/; expires=" + todayDate.toGMTString() + "; domain=" + window.location.hostname;
	console.log(document.cookie);
	
}
function getCookie(name) {
	//var parts = document.cookie.split(name + "=");
	var parts = document.cookie.split(";");
	console.log("parts:"+parts)
	if (parts.length == 2) {
		
		return parts.pop().split(";").shift();
	}
}

var downloadTimer;
function blockLoadingBar() {
	$("#pdfloading").show();
	$("#table_wrap").hide();
	downloadTimer = setInterval(function () {
		var token = getCookie("pdfDownload");
		if (token == "true") {
			UnblockLoadingBar();
		}
	}, 1000);
}
function UnblockLoadingBar() {
	$('#pdfloading').fadeOut('fast');
	$("#table_wrap").show();
	$('body').css('overflow', 'inherit');
	clearInterval(downloadTimer);
	setCookie("pdfDownload", "false");
}
function popupReport(type, id) {
	const url = "/Kr/" + type + "report?id=" + id;
	const name = "Report";
	const option = "width = 920, height = 1200, top = 100, left = 200";
	window.open(url, name, option);
}

function popupReportSelected(type) {
	let id = '';
	let arr = [];
	let checkCount = $('input:checkbox[name="checkbox"]:checked').length;
	if (checkCount == 0) {
		alert('선택된 리스트가 없습니다. No list selected.');
	} else {
		$('input[name="checkbox"]:checked').each(function () {
			arr.push($(this).data('id'));
			id += ($(this).data('id')) + '|';
		});
		const url = "/Kr/" + type + "report?id=" + arr.join('|');
		const name = "Report";
		const option = "width = 920, height = 1200, top = 100, left = 200";
		window.open(url, name, option);
	}
}

function countCheck(e, lang) {
	let alertCount = '';
	let checkCount = $('input:checkbox[name="checkbox"]:checked').length;
	console.log(checkCount);
	if (lang == 'en') {
		alertCount = "Cannot check more than 10 lists.";
	} else {
		alertCount = "10개 이상 체크할 수 없습니다.";
	}
	if (checkCount > 10) {
		alert(alertCount);
		$("input#chk" + (e + 1)).prop("checked", false);
	}
}

function pagination(e, type) {
	$('#pagination > .page' + e).addClass('bold');
	$('#pagination > .paging').not('.page' + e).removeClass('bold');

	var pageConst = (e - 1) * pagePosts;
	$('tr.rows').addClass('none');
	for (i = pageConst + 1; i <= pageConst + pagePosts; i++) {
		$('tr.rows#row' + i).removeClass('none');
	}

}

function movingPage(e, type) {
	currentPage = e;
	pagination(e, type);
}

function pushSeries (type) {
	for(i=0; i<legends[type].length; i++) {
		const dataName = legends[type][i];
		const thisData = {
			[type] : {
				name: dataName,
				type:'scatter',
				coordinateSystem: 'polar',
				symbol:shapes[type],
				data:data[type][dataName]['coordinate'],
				color:color[type],
				animationDelay: function (idx) {return idx * 5;}
			},
		};
		var dataCounts = data[type][dataName]['coordinate'].length
		if(dataCounts > 0) {
			scatterData.push(thisData[type]);
			var legend_elements = '<option value="'+dataName+'" data-type="'+type+'">'+dataName.substr(1)+'% ('+dataCounts+'개)'+'</option>';
			$('#legend_'+type).append(legend_elements);
		}
	}
}

function showChart () {
	$('#loading').fadeOut('fast');
	$('body').css('overflow','inherit');
}

function testRun() {
	pushSeries("patent");
	pushSeries("article");
	initiateChart();
}