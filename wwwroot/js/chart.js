let windowWidth = $(window).width();
let chartWidth;
let chartHeight;

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

function initiateChart (chartType) {
	showChart();
	loadToc('all', 'E00');
	if(chartType == 'scatter') {
		option = scatterOption;
	} else if (chartType == 'bar') {
		option = barOption;
		loadToc("patent", "P00");
	} else if (chartType == 'net') {
		option = netOption;
		loadNetToc("patent");
		loadNetToc("article");
	}
	myChart.setOption(option, true);
	myChart.on('click',function(params) {
		let seriesType = params.seriesType;
		if(seriesType == "graph") {
			let caPicked = params.data["category"];
			let noPicked = params.data["name"];
			if(caPicked != "TechInput") {popupReport(caPicked,noPicked);}
			
		} else if (seriesType == "bar") {
			loadDepthToc(params.name);
		}
	});
}