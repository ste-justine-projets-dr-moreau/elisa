﻿
function setupChosenSelectWithAdd(select, hiddenInputName, hiddenInputId, text, no_results_text, isMultiple) {

    // La solution a été inspirée de ce lien :
    // http://stackoverflow.com/questions/18706735/adding-text-other-than-the-selected-text-options-to-the-select-with-the-chosen-p

    var targetSelect, targetChosen, options;

    targetSelect = $(select);

    options = {
        no_results_text: no_results_text, //Press Enter to add new entry:
        width: "50%",
        disable_search: false
    };

    if (isMultiple) {
        options.placeholder_text_multiple = text;
    } else {
        options.placeholder_text_single = text;
    }

    targetSelect.chosen(options);
    targetChosen = targetSelect.data("chosen");
    targetChosen.dropdown.find('input').on('keyup', function (e) {

        // if we hit Enter and the results list is empty (no matches) add the option
        //if (e.which == 13 && targetChosen.dropdown.find('li.no-results').length > 0) {
        if (e.which == 13) {
            var option = $("<option>").val(0).text(this.value);
            targetSelect.prepend(option);
            targetSelect.find(option).prop('selected', true);

            targetSelect.trigger("chosen:updated");
            targetSelect.change();
        }

    });
    targetSelect.on('change', function () {
        var nameSelected = targetSelect.find(":selected").text();
        var idSelected = targetSelect.find(":selected").val();
        $("[name='" + hiddenInputName + "']").val(nameSelected);
        $("[name='" + hiddenInputId + "']").val(idSelected);
    });
    console.log(targetChosen.dropdown.find('input'));
}