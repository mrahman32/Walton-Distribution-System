function create_top_ten_smartphone_chart(url) {

    $.post(url, function (data) {
        console.log(data);

        var len = data.length;
        if (len > 0) {
            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            // Create chart instance
            var chart = am4core.create("topTenSmartPhone", am4charts.XYChart);

            // Add data
            chart.data = data;

            //    [{
            //    "country": "USA",
            //    "visits": 2025
            //}, {
            //    "country": "China",
            //    "visits": 1882
            //}, {
            //    "country": "Japan",
            //    "visits": 1809
            //}, {
            //    "country": "Germany",
            //    "visits": 1322
            //}, {
            //    "country": "UK",
            //    "visits": 1122
            //}, {
            //    "country": "France",
            //    "visits": 1114
            //}, {
            //    "country": "India",
            //    "visits": 984
            //}, {
            //    "country": "Spain",
            //    "visits": 711
            //}, {
            //    "country": "Netherlands",
            //    "visits": 665
            //}, {
            //    "country": "Russia",
            //    "visits": 580
            //}, {
            //    "country": "South Korea",
            //    "visits": 443
            //}, {
            //    "country": "Canada",
            //    "visits": 441
            //}, {
            //    "country": "Brazil",
            //    "visits": 395
            //}];

            // Create axes

            var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "Model";
            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.renderer.minGridDistance = 30;

            categoryAxis.renderer.labels.template.adapter.add("dy", function (dy, target) {
                if (target.dataItem && target.dataItem.index & 2 == 2) {
                    return dy + 25;
                }
                return dy;
            });

            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

            // Create series
            var series = chart.series.push(new am4charts.ColumnSeries());
            series.dataFields.valueY = "Quantity";
            series.dataFields.categoryX = "Model";
            series.name = "Quantity";
            series.columns.template.tooltipText = "{categoryX}: [bold]{valueY}[/]";
            series.columns.template.fillOpacity = .8;

            var columnTemplate = series.columns.template;
            columnTemplate.strokeWidth = 2;
            columnTemplate.strokeOpacity = 1;
        }
    });
    
}

function create_top_ten_featurephone_chart(url) {
    $.post(url, function (data) {
        console.log(data);

        var len = data.length;
        if (len > 0) {
            // Themes begin
            am4core.useTheme(am4themes_animated);
            // Themes end

            // Create chart instance
            var chart = am4core.create("topTenFeaturePhone", am4charts.XYChart);

            // Add data
            chart.data = data;

            //    [{
            //    "country": "USA",
            //    "visits": 2025
            //}, {
            //    "country": "China",
            //    "visits": 1882
            //}, {
            //    "country": "Japan",
            //    "visits": 1809
            //}, {
            //    "country": "Germany",
            //    "visits": 1322
            //}, {
            //    "country": "UK",
            //    "visits": 1122
            //}, {
            //    "country": "France",
            //    "visits": 1114
            //}, {
            //    "country": "India",
            //    "visits": 984
            //}, {
            //    "country": "Spain",
            //    "visits": 711
            //}, {
            //    "country": "Netherlands",
            //    "visits": 665
            //}, {
            //    "country": "Russia",
            //    "visits": 580
            //}, {
            //    "country": "South Korea",
            //    "visits": 443
            //}, {
            //    "country": "Canada",
            //    "visits": 441
            //}, {
            //    "country": "Brazil",
            //    "visits": 395
            //}];

            // Create axes

            var categoryAxis = chart.xAxes.push(new am4charts.CategoryAxis());
            categoryAxis.dataFields.category = "Model";
            categoryAxis.renderer.grid.template.location = 0;
            categoryAxis.renderer.minGridDistance = 30;

            categoryAxis.renderer.labels.template.adapter.add("dy", function (dy, target) {
                if (target.dataItem && target.dataItem.index & 2 == 2) {
                    return dy + 25;
                }
                return dy;
            });

            var valueAxis = chart.yAxes.push(new am4charts.ValueAxis());

            // Create series
            var series = chart.series.push(new am4charts.ColumnSeries());
            series.dataFields.valueY = "Quantity";
            series.dataFields.categoryX = "Model";
            series.name = "Quantity";
            series.columns.template.tooltipText = "{categoryX}: [bold]{valueY}[/]";
            series.columns.template.fillOpacity = .8;

            var columnTemplate = series.columns.template;
            columnTemplate.strokeWidth = 2;
            columnTemplate.strokeOpacity = 1;
        }
    });


}