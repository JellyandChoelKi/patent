function selectLegend (dataType, e) {
	let legendName = $('#legend_'+dataType).val();
	let legendRange = (((legendName.substr(1)*1)-5) * 0.01);
	if(legendRange == -0.05) {legendRange = 0;}
	$('.legendSelect').not('#legend_'+dataType).val(" ");

	if(dataType == 'all') {
		focusRange(legendRange, 'all');
	} else {					
		for(i=0; i<scatterData.length; i++){
			myChart.dispatchAction({
				type:'legendUnSelect',
				name: scatterData[i]['name'],
			});
		}
		
		focusRange (legendRange, dataType, legendName);
	}
	loadToc(dataType, legendName);
}

function focusRange (zoomLevel, dataType, categoryName) {
	let legendType = "";
	let legendName = "";
	if(dataType == 'all' || zoomLevel == 0){
		legendType = "legendAllSelect";
		legendName = "";
	} else {
		legendType = "legendSelect";
		legendName = categoryName;
	} 
	myChart.dispatchAction({
		type:legendType,
		name:legendName
	});
	myChart.dispatchAction({
		type: 'dataZoom',
		startValue: 1.0,
		endValue:zoomLevel,
	});
}



function loadToc (dataType, dataRange) {
	$('.toc>tbody').html('');
	let legendRange = dataRange.substr(1)*0.01;
	if(dataType == 'all') {
		pushToc('patent', legendRange);
		pushToc('article', legendRange);
		$('#patent_wrap').show();
		$('#article_wrap').show();
	} else {
		pushToc(dataType, legendRange);
		$('.post_wrap').not('#'+dataType+'_wrap').hide();
		$('#'+dataType+'_wrap').show();
	}
}


function pushToc (dataType, legendRange, lang) {	
	let totalLength = data[dataType]['similarity'].length;
	let addRow = "";
	let rowNo = 0;
	let rowData = [];
	for(var i=0; i<totalLength; i++) {
		let thisSimilarity = data[dataType]['similarity'][i];
		if(legendRange == 0){		
			if (thisSimilarity >= legendRange) {
				let thisRow = {
					no: data[dataType]['no'][i],
					title: data[dataType]['title'][i],
					year: data[dataType]['pubyear'][i],
					similarity : (thisSimilarity * 100).toFixed(1),
				}
				rowNo++;
				addRow += '<tr id="row' + rowNo + '" class="rows_' + dataType + '"><td class=""><input onChange="countCheck(' + i + ', \'' + lang + '\')" type="checkbox" name="checkbox" id="chk' + (i + 1) + '" data-id="' + thisRow["no"] + '"></td><td class="no">' + rowNo + '</td><td style="text-align:left;">' + thisRow['title'] + '</td><td>' + thisRow['similarity'] + '</td><td>' + thisRow['year'] + '</td><td><a onClick="popupReport(\'' + dataType + '\', \'' + thisRow['no'] + '\')"><img src="/img/seo.png" style="width:30px;"></a></td></tr>';				
				$('#table_'+dataType+'>tbody').html(addRow);					
			}
		} else {		
			if (thisSimilarity >= legendRange && thisSimilarity < (legendRange+0.1)) {
				let thisRow = {
					no: data[dataType]['no'][i],
					title: data[dataType]['title'][i],
					year: data[dataType]['pubyear'][i],
					similarity : (thisSimilarity * 100).toFixed(1),
				}
				rowNo++;
				addRow += '<tr id="row' + rowNo + '" class="rows_' + dataType + '"><td class=""><input onChange="countCheck(' + i + ', \'' + lang + '\')" type="checkbox" name="checkbox" id="chk' + (i + 1) + '" data-id="' + thisRow["no"] + '"></td><td class="no">' + rowNo + '</td><td style="text-align:left;">' + thisRow['title'] + '</td><td>' + thisRow['similarity'] + '</td><td>' + thisRow['year'] + '</td><td><a onClick="popupReport(\'' + dataType + '\', \'' + thisRow['no'] + '\')"><img src="/img/seo.png" style="width:30px;"></a></td></tr>';			
				$('#table_'+dataType+'>tbody').html(addRow);
			}
		}
	}
	makePage(dataType, rowNo);
}

let currentPage = {
	patent:1,
	article:1,
};

const pagePosts = 30;

function makePage (dataType, totalCount) {
	let pageCount = {
		patent:{count:'',links:''},
		article:{count:'',links:''},
	};
	pageCount[dataType]['count'] = Math.ceil(totalCount / pagePosts);
	for(var p = 1; p<=pageCount[dataType]['count']; p++) {
		pageCount[dataType]['links'] += '<span class="pad10 paging page'+p+'" onClick=movingPage('+p+',"'+dataType+'")>'+p+'</span>';
	}
	$('#page_'+dataType).html(pageCount[dataType]['links']);
	pagination(currentPage[dataType], dataType);
	//console.log(currentPage[dataType] +":"+ dataType);
}

function pagination (pageNow, dataType) {
	$('#page_'+dataType+'> .page'+pageNow).addClass('bold');
	$('#page_'+dataType+'> .paging').not('.page'+pageNow).removeClass('bold');

	var pageConst = (pageNow-1)*pagePosts;
	$('tr.rows_'+dataType).addClass('none');
	for(i = pageConst+1; i <= pageConst + pagePosts; i++) {
		$('tr.rows_'+dataType+'#row'+i).removeClass('none');
	}
}

function movingPage (targetPage, dataType) {
	pagination(targetPage, dataType);
}