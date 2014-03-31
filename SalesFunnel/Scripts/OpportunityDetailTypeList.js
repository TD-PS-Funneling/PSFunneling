var hiddenFieldResourceID;
var hiddenFieldMode;

function addResource() {
    //editResource("0");
    setHiddenField(hiddenFieldResourceID, 0);
    setHiddenField(hiddenFieldMode, "Add");
    

    $("#frmSiteMaster").submit();
}

function deleteResource() {
    validateDelete();
}

function validateDelete() {
    var countCheck = 0;
    $('input:checkbox[name$=chkSelect]').each(
        function () {
            if ($(this).is(':checked') == true)
                countCheck++;
        });

    if (countCheck > 0) {
        //$('#confirm').dialog("open");
        var result = confirm("Are you sure you want to delete these records?")
        if (result == true) {
            var deleteitems = "";

            $('input:checkbox[name$=chkSelect]').each(
            function () {
                if ($(this).is(':checked') == true) {
                    var strID = $(this).attr("id");
                    strID = strID.replace("chkSelect", "hdnID");
                    deleteitems = $("#" + strID).val(); // + "_";
                    webDeleteResource(deleteitems);
                }

            });

            $("#frmSiteMaster").submit();
            alert("Record(s) successfully deleted.");
            //window.location.href = "ResourceList.aspx"
        }
        return false;
    }
    else {
        alert("Please select a record to delete");
        return false;
    }
}

function webDeleteResource(oppID) {
    $.ajax({
        cache: false,
        type: 'POST',
        url: 'FunnelAppSvc.asmx/DelUtilOppDetail',
        data: '{ResourceID:' + oppID + '}',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (response) {
            //alert(response.d);
            //window.location.href = "ResourceList.aspx"
            return true;
        },
        error: function (xhRequest, ErrorText, thrownError) {
            alert(xhRequest.responseText)
            //alert("Error while deleting records")
        }
    });
}

function setAllElements(chkAll) {

    var isChecked = $("#" + chkAll).is(':checked');// ? true : false;
    if (isChecked) {
        $('input:checkbox[name$=chkSelect]').each(
                function () {
                    $(this).prop('checked', true);
                });
    }
    else {
        $('input:checkbox[name$=chkSelect]').each(
                function () {
                    $(this).prop('checked', false);
                });
    }
}

function viewResource(resourceID) {
    setHiddenField(hiddenFieldResourceID, resourceID);
    setHiddenField(hiddenFieldMode, "View");
}

function editResource(resourceID) {
    setHiddenField(hiddenFieldResourceID, resourceID);
    setHiddenField(hiddenFieldMode, "Edit");
}

function setHiddenField(hiddenFieldID, value) {
    $("input[id=" + hiddenFieldID + "]").val(value);
}

function cancelClick() {

    var resourceID = $("input[id=" + hiddenFieldResourceID + "]").val();
    var mode = $("input[id=" + hiddenFieldMode + "]").val();

    if (resourceID != "0" && mode == "Edit") {
        viewResource(resourceID);
    }
    else {
        viewResource("-1");
    }

}

function editClick() {
    var resourceID = $("input[id=" + hiddenFieldResourceID + "]").val();
    editResource(resourceID);
}

function saveResource() {
    var resourceID = $("input[id=" + hiddenFieldResourceID + "]").val();
    var confirmation = confirm("Save Information?");

    if (confirmation) {
        viewResource(resourceID);

        return true;

    }
    else
        return false;
}