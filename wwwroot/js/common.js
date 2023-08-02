const color = {
	patent: '#CD1F48',
	article: '#0064FF'
}

let getParameters = function (paramName) { 
	var returnValue; 
	var url = location.href; 
	var parameters = (url.slice(url.indexOf('?') + 1, url.length)).split('&'); 
	for (var i = 0; i < parameters.length; i++) { 
		var varName = parameters[i].split('=')[0]; 
		if (varName.toUpperCase() == paramName.toUpperCase()) { 
			returnValue = parameters[i].split('=')[1]; 
			return decodeURIComponent(returnValue); 
		}
	}
};
	
let keywords = getParameters('keywords');

function underconstruction (e) {
	if(e == 'en') {alert ('Coming soon.');} else {alert ('준비중입니다.');}
}

function refresh() {
	location.reload();
}

function hamburger () {
	$('#hamburger').toggleClass('active');
	$('#mnav').slideToggle("fast");
}