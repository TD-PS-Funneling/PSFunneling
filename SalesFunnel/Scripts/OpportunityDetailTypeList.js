var hiddenFieldResourceID;
var hiddenFieldMode;

function addResource() {
    //editResource("0");
    setHiddenField(hiddenFieldResourceID, 0);
    setHiddenField(hiddenFieldMode, "Add");
    

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