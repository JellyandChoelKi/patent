let scatterData = [];

function getRandomInt(min, max) {
  min = Math.ceil(min);
  max = Math.floor(max);
  return Math.floor(Math.random() * (max - min)) + min; //최댓값은 제외, 최솟값은 포함
}

let scatterTemp = [];
let scatterKeys = [];
function pushScatterSeries (dataType) {
	let dataNow = data[dataType];
	let thisName;
	let thisRange;
	let thisType;
	let coordinate = [];
	let randomPosition;
	for(var n=0; n<dataNow["no"].length; n++) { // 해당 계열 내 항목 갯수만큼 반복
		let thisSimularity = dataNow["similarity"][n];		
		if(dataType == "patent") {thisType = "P";} else if(dataType == "article"){thisType = "A";}
		
		if(thisSimularity >= 0.90) {
			thisRange = "90";
		} else if (thisSimularity >= 0.80) {
			thisRange = "80";
		} else if (thisSimularity >= 0.70) {
			thisRange = "70";
		} else if (thisSimularity >= 0.60) {
			thisRange = "60";
		} else if (thisSimularity >= 0.50) {
			thisRange = "50";
		} else if (thisSimularity >= 0.40) {
			thisRange = "40";
		} else if (thisSimularity >= 0.30) {
			thisRange = "30";
		} else if (thisSimularity >= 0.20) {
			thisRange = "20";
		} else if (thisSimularity >= 0.10) {
			thisRange = "10";
		} else if (thisSimularity >= 0.0) {
			thisRange = "00";
		}
		thisName = thisType + thisRange
		if(coordinate[thisName] == undefined) {coordinate[thisName]= new Array();}
		randomPosition = getRandomInt(1, 360);
		coordinate[thisName].push([thisSimularity, randomPosition]);
		let thisData = {
			name: thisName,
			type: 'scatter',
			coordinateSystem : 'polar',
			data: coordinate[thisName],
			itemStyle: {
				color:color[dataType],
			}
		};
		if(scatterTemp[thisName] == undefined) {
			scatterTemp[thisName] = new Array();
			scatterData.push(thisData);	
			$("#legend_"+dataType).append('<option value="'+thisType+thisRange+'">'+thisRange+'%</option>');
			console.log(dataType +" : "+ thisRange);			
		}
		
		if(scatterKeys[thisRange] == undefined) {
			scatterKeys[thisRange] = new Array();
			$("#legend_all #E"+thisRange).removeClass('none');
		}
	}
}

let scatterOption = {
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
  series: scatterData,
};