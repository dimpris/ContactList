// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(() => { 
    initPhoneNumbersEditor();
});

function initPhoneNumbersEditor() {
    const addBtn = $('#add-phone-number-btn');
    const editor = $('#phone-numbers-editor');

    if (addBtn.length) {
        if (editor.data('phonesCount') == 0) {
            addPhoneNumberRow();
        }

        $('#phone-numbers-editor').on('click', '.remove-phone-number-btn', function(){
            $(this).closest('.row').remove();
        });

        $('#phone-numbers-editor').on('click', '.save-phone-number-btn', function () {
            // TODO implement AJAX saving
            $(this).closest('.row').removeClass('edit');
        });

        $('#phone-numbers-editor').on('click', '.edit-phone-number-btn', function () {
            const el = $(this);
            const phoneType = el.data('type');
            const opt = el.closest('.row').find(`select option[value=${phoneType}]`);
            opt.attr('selected', true);
            el.closest('.row').addClass('edit');
        });

        $('#phone-numbers-editor').on('click', '.delete-phone-number-btn', function () {
            // TODO implement AJAX delete
            $(this).closest('.row').remove();
        });

        addBtn.on('click', (ev) => {
            addPhoneNumberRow();
        });
    }
}

function addPhoneNumberRow() {
    const rowTemplate = $('#phone-number-template');
    const editor = $('#phone-numbers-editor');

    if (rowTemplate.length) {
        let rowClone = rowTemplate.clone();
        rowClone.removeClass('d-none').addClass('edit');
        editor.append(rowClone);
    }
}