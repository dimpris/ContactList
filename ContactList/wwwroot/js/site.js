// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(() => { 
    initPhoneNumbersEditor();
    initMaskedFields();
});

function initMaskedFields() {
    if ($("*[mask]").length) {
        $('[mask]').each(function (e) {
            $(this).mask($(this).attr('mask'));
        });
    }
}
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
            const el = $(this);
            const id = el.data('id');
            const row = el.closest('.row');
            const phone_number_editor = row.find("input[name=phone_number_editor]");
            const phone_type_editor = row.find("select[name=phone_type_editor]");

            const number = phone_number_editor.val();
            const type = phone_type_editor.val();


            const phone_number_view = row.find(".phone_number_view a");
            const phone_type_view = row.find(".phone_type_view");
            phone_number_view.attr('href', 'tel:' + number).text(number);
            const type_text = phone_type_editor.find(`option[value=${type}]`).text();
            phone_type_view.text(type_text);
            phone_number_editor.prop('disabled', true);
            phone_type_editor.prop('disabled', true);
            const editData = {
                Number: number,
                Type: parseInt(type)
            };

            apiPost("/api/ContactList/EditPhone/" + id, editData, res => {
                if (res == true) {
                    row.removeClass('edit');
                    phone_number_editor.prop('disabled', false);
                    phone_type_editor.prop('disabled', false);
                }
            });
        });

        $('#phone-numbers-editor').on('click', '.edit-phone-number-btn', function () {
            const el = $(this);
            const phoneType = el.data('type');
            const opt = el.closest('.row').find(`select option[value=${phoneType}]`);
            opt.attr('selected', true);
            el.closest('.row').addClass('edit');
        });

        $('#phone-numbers-editor').on('click', '.delete-phone-number-btn', function () {
            const el = $(this);
            const id = el.data('id');
            apiPost("/api/ContactList/DeletePhone/" + id, {}, res => {
                $(this).closest('.row').remove();
            });
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
        let maskedInputs = rowClone.find('input[mask]');
        maskedInputs.each(function (e) {
            $(this).mask($(this).attr('mask'));
        });
        editor.append(rowClone);
    }
}

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}
function apiQuery(url, method, onDone, data = null) {
    const token = getCookie("accessToken") || "";
    var settings = {
        "url": url,
        "method": method,
        dataType: "json",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
    };

    if (data) {
        settings.data = JSON.stringify(data);
    }

    $.ajax(settings).done(function (response) {
        if (onDone && typeof onDone == 'function') {
            onDone(response);
        }
    });
}

function apiGet(url, onDone) {
    apiQuery(url, "GET", onDone);
}

function apiPost(url, data, onDone) {
    apiQuery(url, "POST", onDone, data);
}