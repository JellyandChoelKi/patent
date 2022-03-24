function totalAjax() {
        $.when(
            $.ajax({ // 특허 API
			url: 'https://k2gdoc2vec.azurewebsites.net/patentData?searchWord=' + '@ViewBag.PatentKeyword',
            type: "GET",
            dataType: "JSON",
            beforeSend: function () {
                console.log("Patent Start");
                $('#loading>div#status').append('<p>특허데이터를 분석 중입니다.</p>');
            },
            success: function (result) {
                console.log('Patent Success');
                $('#loading>div#status').append('<p>특허데이터 분석이 완료되었습니다.</p>');
                data["patent"] = result;
                pushScatterSeries("patent");
            },
            error: function () {
                $('#loading>div#status').append('<p style="color:#C00;">특허데이터 획득에 실패하였습니다.</p>');
            },
            complete: function () {
                console.log('Patent End');
                //searchArticles();
            }
        }),
            $.ajax({ // 논문 API
            url: 'https://k2gdoc2vec.azurewebsites.net/thesisData?searchWord=' + '@ViewBag.ArticleKeyword',
            type: "GET",
            dataType: "JSON",
            beforeSend: function () {
                console.log("Article Start");
                $('#loading>div#status').append('<p>논문 데이터를 분석 중입니다.</p>')
            },
            success: function (result) {
                console.log('Article Success');
                $('#loading>div#status').append('<p>논문데이터 분석이 완료되었습니다.</p>')
                data["article"] = result;
                pushScatterSeries("article");
            },
            error: function () {
                $('#loading>div#status').append('<p style="color:#C00;">논문데이터 분석이 실패하였습니다.</p>');
            },
            complete: function () {
                console.log('Article Ended');
            }
        }),			

        ).done(function () {
			initiateChart("scatter");
        })
    }