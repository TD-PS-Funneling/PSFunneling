//Begin - Javscript functions

//these variables are being populated on the markup of the html page
var hiddenFieldResourceID; //contains the client id of the hidden field for the resource id 
var hiddenFieldMode;   //contains the client id of the hidden field for the mode

function addResource(){
    editResource("0");

    $("#frmSiteMaster").submit();
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
            //window.location.href = "ResourceList.aspx"
        }
        return false;
    }
    else {
        alert("Please select a record to delete");
        return false;
    } 
}

function deleteResource() {
    validateDelete();
}

function viewResource(resourceID) {
    setHiddenField(hiddenFieldResourceID, resourceID);
    setHiddenField(hiddenFieldMode, "View");
}

function editResource(resourceID) {
    setHiddenField(hiddenFieldResourceID, resourceID);
    setHiddenField(hiddenFieldMode, "Edit");
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
    var confirmation = confirm("Save Resource Information?");

    if (confirmation) {
        viewResource(resourceID);

        return true;

    }
    else
        return false;
}

function setHiddenField(hiddenFieldID, value) {
    $("input[id=" + hiddenFieldID + "]").val(value);
}
//End - Javscript functions

//Begin - Ajax Web Services Call

function webDeleteResource(resourceids) {
    $.ajax({
        cache: false,
        type: 'POST',
        url: 'FunnelAppSvc.asmx/DelResource',
        data: '{ResourceID:' + resourceids + '}',
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
//End - Ajax Web Services Call


//Begin - Initializes the message dialogs.
//$(document).ready(function () {

//    $('#confirm').dialog({
//        resizable: false,
//        height: 120,
//        width: 285,
//        modal: true,
//        autoOpen: false,
//        buttons: {
//            Yes: function () {
//                var deleteitems = "";

//                $('input:checkbox[name$=chkSelect]').each(
//                function () {
//                    if ($(this).is(':checked') == true) {
//                        var strID = $(this).attr("id");
//                        strID = strID.replace("chkSelect", "hdnID");
//                        deleteitems = deleteitems + $("#" + strID).val() + "::";
//                    }
                       
//                });

//                deleteitems = deleteitems.substr(0, deleteitems.length - 2);
//                window.location.href = "ResourceList.aspx?items=" + deleteitems;
//            },
//            No: function () {
//                $(this).dialog('close');
//            }
//        }
//    });
//});
//End - Initializes the message dialogs.