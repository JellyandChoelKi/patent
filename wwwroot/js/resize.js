var windowWidth = $(window).width();
var chartWidth;
var chartHeight;

if(windowWidth > 900) {
	chartWidth = 800;
	chartHeight = 800;
} else {
	chartWidth = windowWidth * 0.8;
	chartHeight = chartWidth * 1.1;
}

var myChart = echarts.init(document.getElementById('graph'), null, {
	width: chartWidth,
	height: chartHeight,
});	