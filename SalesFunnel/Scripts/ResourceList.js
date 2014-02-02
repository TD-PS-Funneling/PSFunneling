//Begin - Javscript functions
function addResource() {
    window.location.href = "Resource.aspx?ID=0";
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
                    deleteitems = deleteitems + $("#" + strID).val() + "::";
                }

            });

            deleteitems = deleteitems.substr(0, deleteitems.length - 2);
            window.location.href = "ResourceList.aspx?items=" + deleteitems;
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


//End - Javscript functions

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