var option = {
  title: {
	text: ''
  },
  legend: { 
	  show:false,
  },
polar: {
	center: ['50%', '50%'],
},
angleAxis: {
	min: 0,
	max: 360,
	interval: 18,
	boundaryGap: false,
	splitLine: {
	  show: true
	},
	axisLabel : {
	  show : false
	},
	axisLine: {
	  show: false
	},
	axisTick: {
		show: false
	},
},
tooltip: {
	  formatter: function (params) {
		  return  'Similarity:' + ((params.value[0] * 100).toFixed(1))+'%';
	  }
},
radiusAxis: {
		min: 0,
		max: 1,
		interval : 0.1,
	axisLine: {
		show: true
	},
	axisLabel: 
	{
		formatter: function (value, index) {
			return (value * 100).toFixed(1);
		}
	},
		inverse: true,
	splitArea : 
	{
		show : true,
	}
  },

  dataZoom: [
	{
		id:'dataZoomX',
		type:'slider',
		radiusAxisIndex: [0],
		handleSize: '100%',
		height: 30,
		borderColor: "#999",
		backgroundColor:"#EEE",
		fillerColor:"#CCC",
	}
  ],
  series: series,
};