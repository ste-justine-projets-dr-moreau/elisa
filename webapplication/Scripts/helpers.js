
function setupChosenSelectWithAdd(select, hiddenInputName, hiddenInputId, text, no_results_text, isMultiple) {

    // La solution a été inspirée de ce lien :
    // http://stackoverflow.com/questions/18706735/adding-text-other-than-the-selected-text-options-to-the-select-with-the-chosen-p


    var targetSelect,
        targetChosen,
        options,
        userInput,
        idCounter = 0,
        addingItem = false;


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


    if (isMultiple) {
        userInput = targetSelect
                        .parent()
                        .find("input");
    } else {
        userInput = targetChosen
                        .dropdown
                        .find('input');
                        
    }

    userInput.on('keyup', function (e) {

        // if we hit Enter and the results list is empty (no matches) add the option
        if (e.which == 13 && targetChosen.dropdown.find('li.no-results').length > 0) {

            if (isMultiple) { idCounter--; }

            var option = $("<option>")
                                .val(idCounter)
                                .text(this.value);

            if (isMultiple) {
                targetSelect.append(option);
                addingItem = true;
            } else {
                targetSelect.prepend(option);
            }
            
            targetSelect.find(option).prop('selected', true);

            targetSelect.trigger("chosen:updated");
            targetSelect.change();
        }

    });

    targetSelect.on('change', function () {

        var hiddenInput = $("[name='" + hiddenInputName + "']");
        var valueForHiddenInputName = "";

        var nameSelected;
        var idSelected;

        if (isMultiple) {
            nameSelected = targetSelect.find(":selected").last().text();
            idSelected = targetSelect.find(":selected").last().val();
        } else {
            nameSelected = targetSelect.find(":selected").text();
            idSelected = targetSelect.find(":selected").val();
        }


        var currentValueForHiddenInputName = hiddenInput.val();

        if (isMultiple) {

            if (addingItem) {
                if (currentValueForHiddenInputName && currentValueForHiddenInputName != "") {
                    valueForHiddenInputName = currentValueForHiddenInputName
                                                + "^^^"
                                                + nameSelected;
                } else {
                    valueForHiddenInputName = nameSelected;
                }

                hiddenInput.val(valueForHiddenInputName);
                addingItem = false;
            }

        } else {
            hiddenInput.val(nameSelected);
            $("[name='" + hiddenInputId + "']").val(idSelected);
        }
        
        

    });
    
}