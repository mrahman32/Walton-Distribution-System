function LoadModel(url) {

    var selectedValue = $('#ProductTypeId').val();
    $.get(url, {phoneTypeId:selectedValue}, function (data) {
        console.log(data);
        $("#ModelName").empty();
        $("#ModelName").append($("<option     />").val("").text("Please Select"));
        $.each(data, function (index, value) {
            console.log("value:" + this.Value + "--- Text:" + this.Text);
            $("#ModelName").append('<option id="' + value.Value + '">' + value.Text + '</option>');
        });
        $('.selectpicker').selectpicker('refresh');
    });
}


function removeDetailRow(element) {
    var detailRow = $(element).closest('tr');
    $(detailRow).remove();
    SumTotal();
}

function SumTotal() {
    //debugger;
    var result = 0;
    var tableElem = window.document.getElementById("productTable");
    var tableBody = tableElem.getElementsByTagName("tbody").item(0);
    var i;
    var howManyRows = tableBody.rows.length;
    for (i = 0; i < (howManyRows) ; i++) {
        var thisTrElem = tableBody.rows[i];
        var thisTdElem = thisTrElem.cells[3];
        var thisTextNode = thisTdElem.childNodes.item(0);
        var thisNumber = parseFloat(thisTextNode.data);
        if (!isNaN(thisNumber)) {
            result += thisNumber;

        }

    }
    $('#GrandTotal').val(result);
}