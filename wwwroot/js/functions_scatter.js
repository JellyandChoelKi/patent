const scatterData = [];
const shapes = {
	patent : 'circle',
	article : 'rect',
};

function selectLegend (type, e) {
	let legendName = $('#legend_'+type).val();
	let legendRange = (((legendName.substr(1)*1)-5) * 0.01);
	
	$('.legendSelect').not('#legend_'+type).val("");
	
	if(legendName != ''){
		if(type == 'all') {
			focusRange(legendRange, 'all');
		} else {					
			for(i=0; i<scatterData.length; i++){
				myChart.dispatchAction({
					type:'legendUnSelect',
					name: scatterData[i]['name'],
				});
			}

			myChart.dispatchAction({
				type:'legendSelect',
				name: legendName,
			});
			if(e == 'en') {loadToc(legendName, type, 'en')} else {loadToc(legendName, type);}
			focusRange (legendRange);
		}
	}
}

function legendAll (){
	myChart.dispatchAction({
		type:'legendAllSelect',
	});
	focusRange (0);
	loadAll ();
}

function focusRange (a, type) {
	console.log(a);
	if(type == 'all'){
		myChart.dispatchAction({
			type:'legendAllSelect',
		});
	}
	myChart.dispatchAction({
		type: 'dataZoom',
		startValue: 1.0,
		endValue:a,
	});
}