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

function showChart () {
	$('#loading').fadeOut('fast');
	$('body').css('overflow','inherit');
}