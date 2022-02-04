function underconstruction () {			
	alert ('준비중입니다.');
}

function showChart() {
	$('#loading').fadeOut('fast');
	$('body').css('overflow', 'inherit');
}

function hamburger() {
	$('#hamburger').toggleClass('active');
	$('#mnav').slideToggle("fast");
}