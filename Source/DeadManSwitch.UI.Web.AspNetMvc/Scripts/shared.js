function postDelete(msg, href) {
    confirmThenPost(msg, href);

    //Return false so that hyperlink doesn't execute GET
    return false;
}

function confirmThenPost(msg, href) {
    bootbox.confirm(msg, function (result) {
        if (result === true) {
            postUrlAndRedirect(href);
        }
    });
}

function postUrlAndRedirect(href) {
    $.ajax({
        type: "POST",
        url: href,
        success: function (result) {
            window.location.href = result.redirectUrl;
        }
    });
}

function postCheckin(href) {
    postUrlAndDisplayResult(href);

    //Return false so that hyperlink doesn't execute GET
    return false;
}

function postUrlAndDisplayResult(href) {
    $.ajax({
        type: "POST",
        url: href,
        success: function (result) {
            bootbox.alert(result.message);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            bootbox.alert("Check in failed.");
        }
    });
}

//keep two checkboxes in sync. Useful when displaying/hiding different checkboxes based on device size
//these suffixes also exist in CheckBoxToggleButtonModel
var chkBoxSuffix = "";
var toggleBtnSuffix = "ToggleBtn";
function syncCheckBox(toggleBtn) {
    var chkBoxId = swapSuffix(toggleBtn.id, toggleBtnSuffix, chkBoxSuffix);
    setChecked(chkBoxId, toggleBtn.checked);
}

function syncToggleButton(chkBox) {
    var btnId = swapSuffix(chkBox.id, chkBoxSuffix, toggleBtnSuffix);
    setChecked(btnId, chkBox.checked);
}

function swapSuffix(value, oldSuffix, newSuffix) {
    var swapped = value.slice(0, (oldSuffix.length * -1));
    swapped += newSuffix;
    return swapped;
}

function setChecked(id, value) {
    var other = document.getElementById(id);
    if (other) {
        other.checked = value;
    }
}


