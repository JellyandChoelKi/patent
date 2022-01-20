function selectLegend () {
	var legendName = $('#custom_legend').val();			
	var type = $('#custom_legend').find("option:selected").data("type");
	var legendRange = (((legendName.substr(1)*1)-5) * 0.01);
	if(legendName != ''){
		if(legendName == 'all') {
			legendAll();
		} else {						
			for(i=0; i<series.length; i++){
				myChart.dispatchAction({
					type:'legendUnSelect',
					name: series[i]['name'],
				});
			}

			myChart.dispatchAction({
				type:'legendSelect',
				name: legendName,
			});
			loadToc(legendName, type);
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

function loadToc (legendName, type) {
	var currentPage = 1;
	var dataFrom = data[type][legendName];
	var pageData = '';
	var rowData = '';
	var tableTitle = '';

	if (type == 'patent') {
		var tableTitle = "특허";
		var tableHead = "<th class='no'><input type='checkbox' class='chkall' id='chkpat'></th><th class='title'>발명제목</th><th class='author'>출원인</th><th class='year'>출원일</th><th class='report'>리포트</th>";
		var subject ="inventionname";
		var author ="inventors";
		var time = "applicationdate";		
		var id = "applicationno";
	} else if (type == 'article') {
		var tableTitle = "논문";
		var tableHead = "<th class='no'><input type='checkbox' class='chkall' id='chkart'></th><th class='title'>논문제목</th><th class='author'>저자</th><th class='year'>연도</th><th class='report'>리포트</th>";
		var subject ="title";
		var author ="author";
		var time = "pubyear";
		var id = "articleid";
	}
	$('#table_wrap > table').html('');
	var dataCount = dataFrom["coordinate"].length;
	var pageCount = parseInt(dataCount/pagePosts) + 1;
	if(dataCount > 0){
		$('#table_wrap > table').html(tableHead);
		for(i=0; i<dataCount; i++){
			$('#table_wrap > table').append('<tr class="rows" id="row'+(i+1)+'"><td><input type="checkbox" id="chk"'+(i+1)+'></td><td><a class="href" href="" target="_blank">'+dataFrom[subject][i]+'</a></td><td>'+dataFrom[author][i]+'</td><td>'+dataFrom[time][i]+'</td><td><a href="/kr/report?type='+type+'&id='+dataFrom[id][i]+'" class="report" target="_blank">리포트</a></td></tr>');
		}
	} else if(dataCount == 0) {
		$('#table_wrap > table').html('<tr><th>해당하는 항목이 없습니다.</th></tr>');
	}
	for(i=0; i<pageCount; i++) {
		pageData += '<span onClick=movingPage('+(i+1)+',\''+type+'\') class="pad10 paging page'+(i+1)+'">'+(i+1)+'</span>';
	}
	$('#pagination').html(pageData);
	pagination (currentPage, type);
}

function loadAll () {
}

function pagination (e, type) {
	$('#pagination > .page'+e).addClass('bold');
	$('#pagination > .paging').not('.page'+e).removeClass('bold');

	var pageConst = (e-1)*pagePosts;
	$('tr.rows').addClass('none');
	for(i = pageConst+1; i <= pageConst + pagePosts; i++) {
		$('tr.rows#row'+i).removeClass('none');
	}

}

function movingPage (e, type) {
	currentPage = e;
	pagination(e, type);
}

function focusRange (a) {
	myChart.dispatchAction({
		type: 'dataZoom',
		startValue: 1.0,
		endValue:a,
	});
}