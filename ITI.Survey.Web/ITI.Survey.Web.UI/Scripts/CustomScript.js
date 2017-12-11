function printDiv(divName) {
    var header = document.getElementsByTagName('head');
    var divToPrint = document.getElementById(divName);
    var newWin = window.open('', 'Print-Window', '_blank');
    newWin.document.open();
    newWin.document.write('<html><head>' + header[0]['innerHTML'] + '</head><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
    newWin.document.close();
}

$(window).scroll(function () {
    $('#loadingposition').css('top', ($(window).scrollTop() + $(window).height() / 2) - 40);
});

function loading() {
    $('#loadingposition').css('top', ($(window).scrollTop() + $(window).height() / 2) - 40);
    $("body").append('<div class="modal-backdrop fade in" style="z-index: 1054;"></div>')
    $("#divLoading").show();
    setTimeout(function () {
        $("#divLoading").hide();
        $("body").find('div[class="modal-backdrop fade in"]').remove();
    }, 500);
}

function PopupMessage(message, onOkEvent) {
    $('.notif-message').text(message);
    $(".notificationdialog").show();
    $(".notificationdialog").dialog({
        closeOnEscape: false,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
        },
        dialogClass: "no-close",
        resizable: false,
        modal: true,
        autoOpen: false,
        width: 300,
        buttons: {
            OK: function () {
                if (onOkEvent == null || onOkEvent == undefined) {
                    $(this).dialog().dialog("close");
                }
                else {
                    onOkEvent();
                    $(this).dialog().dialog("close");
                }
            }
        }
    });
    $(".notificationdialog").dialog().dialog("open");
}

$.fn.Confirmation = function (onOkEvent) {
    $(this).click(function () {
        var successmsg = $(this).attr('data-success-msg');
        var failedmsg = $(this).attr('data-failed-msg');
        var href = $(this).attr('data-href');
        var form = $(this).closest('form');

        $('#ErrorSummary', form).html('');

        $('.confirm-message').text($(this).attr('data-confirm-msg'));
        $(".confirmationdialog").show();
        $(".confirmationdialog").dialog({
            closeOnEscape: false,
            open: function (event, ui) {
                $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
            },
            dialogClass: "no-close",
            resizable: false,
            modal: true,
            autoOpen: false,
            width: 300,
            buttons: [
                {
                    text: "Yes",
                    width: '50px',
                    click: function () {
                        onOkEvent(href, form, successmsg, failedmsg);
                        $(this).dialog().dialog("close");
                    }
                },
                {
                    text: "No",
                    width: '50px',
                    click: function () {
                        $(this).dialog().dialog("close");
                    }
                }
            ]
        });
        $(".confirmationdialog").dialog().dialog("open");
    });
}

$('input[type="number"]').bind('keydown', function (event) {
    switch (event.keyCode) {
        case 8:  // Backspace
        case 9:  // Tab
        case 13: // Enter
        case 37: // Left
        case 38: // Up
        case 39: // Right
        case 40: // Down
            break;
        default:
            var regex = new RegExp("^[0-9]+$");
            var key = event.key;
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
            break;
    }
});

$.widget("ui.dialog", $.ui.dialog,
{
    open: function () {
        var $dialog = $(this.element[0]);

        var maxZ = 0;
        $('*').each(function () {
            var thisZ = $(this).css('zIndex');
            thisZ = (thisZ === 'auto' ? (Number(maxZ) + 1) : thisZ);
            if (thisZ > maxZ) maxZ = thisZ;
        });

        $(".ui-widget-overlay").css("zIndex", (maxZ + 1));
        $dialog.parent().css("zIndex", (maxZ + 2));

        return this._super();
    }
});

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

$.fn.showloading = function () {
    this.append('<div class="modal-backdrop fade in" style="z-index: 1054;"></div>')
    $("#divLoading").show();
}

$.fn.hideloading = function () {
    $("#divLoading").hide();
    this.find('div[class="modal-backdrop fade in"]').remove();
}

$.fn.showErrorModelState = function () {
    window.scrollTo(0, 0);
    $('form > div > ul > li').click(function () {
        //$('[data-val-required="' + $(this).html() + '"]').focus();
        if ($('[name="' + $(this).attr('key') + '"]').hasClass('ckeditor')) jQuery('.cke_wysiwyg_frame').contents().find('body').focus();
        else
            $('[name="' + $(this).attr('key') + '"]').focus();
    });
    $('form > div > div > ul > li').click(function () {
        //$('[data-val-required="' + $(this).html() + '"]').focus();
        if ($('[name="' + $(this).attr('key') + '"]').hasClass('ckeditor')) jQuery('.cke_wysiwyg_frame').contents().find('body').focus();
        else
            $('[name="' + $(this).attr('key') + '"]').focus();
    });
}

//$.ajaxSetup({
//    error: function (request, status, error) {
//        if ($(request.responseText).find('title') != undefined)
//            PopupMessage($(request.responseText).find('title').html());
//        else
//            PopupMessage("Something went wrong.");
//    }
//});

$(function () {

    // setting regional datepicker
    $.datepicker.regional["id-ID"] = {
        monthNames: ['Januari', 'Februari', 'Maret', 'April', 'Mei', 'Juni', 'Juli', 'Agustus', 'September', 'Oktober', 'November', 'Desember'],
        monthNamesShort: ['Jan', 'Feb', 'Mar', 'Apr', 'Mei', 'Jun', 'Jul', 'Agu', 'Sep', 'Okt', 'Nov', 'Des'],
        dayNames: ['Minggu', 'Senin', 'Selasa', 'Rabu', 'Kamis', 'Jumat', 'Sabtu'],
        dayNamesShort: ['Min', 'Sen', 'Sel', 'Rab', 'Kam', 'Jum', 'Sab'],
        dayNamesMin: ['M', 'S', 'S', 'R', 'K', 'J', 'S']
    }

    $.datepicker.setDefaults($.datepicker.regional["id-ID"]);

    // datepicker cannot typing
    $(':input.hasDatepicker').each(function () {


        $(this).keypress(function (e) {
            var key = e.charCode || e.keyCode;

            if (key == 13) {
                // enter key do nothing
            }
            else {
                e.preventDefault();
            }
        });

        $(this).keydown(function (e) {
            var key = e.charCode || e.keyCode;

            if (key == 13) {
                // enter key do nothing
            }
            else {
                e.preventDefault();
            }
        });

        $(this).keyup(function (e) {
            var key = e.charCode || e.keyCode;

            if (key == 13) {
                // enter key do nothing
            }
            else {
                e.preventDefault();
            }
        });
    });

});

//Returning ajax result from controller in json with resultOperation, message and url for complete usage and file upload
$.fn.StandardSaveOperationWithFile = function (preProcess, objForm, event, urlAction, completeEvent) {
    $(this).on(event, function () {
        if (preProcess != undefined && $.type(preProcess) === 'function')
            preProcess();

        var thisForm = objForm;
        objForm.ajaxSubmit({
            url: urlAction,
            data: thisForm.serialize(),
            type: 'POST',
            beforeSend: function () {
                objForm.showloading();
            },
            complete: function () {
                objForm.hideloading();
            },
            error: function (request, status, error) {
                PopupMessage($(request.responseText).find('title').html());
            },
            success: function (result) {
                if (result.status) {
                    PopupMessage(result.message, function () {
                        if (completeEvent != undefined)
                            completeEvent();
                    });
                } else {
                    PopupMessage(result.message, function () {
                    });
                }
            }
        });
    });
};

function InitializeFunctionUpload() {
    //Returning ajax result from controller in json with resultOperation, message and url for complete usage and file upload
    $.fn.StandardSaveOperationWithFile = function (preProcess, objForm, event, urlAction, completeEvent) {
        $(this).on(event, function () {
            if (preProcess != undefined && $.type(preProcess) === 'function')
                preProcess();

            var thisForm = objForm;
            objForm.ajaxSubmit({
                url: urlAction,
                data: thisForm.serialize(),
                type: 'POST',
                beforeSend: function () {
                    objForm.showloading();
                },
                complete: function () {
                    objForm.hideloading();
                },
                error: function (request, status, error) {
                    PopupMessage($(request.responseText).find('title').html());
                },
                success: function (result) {
                    if (result.status) {
                        PopupMessage(result.message, function () {
                            if (completeEvent != undefined)
                                completeEvent();
                        });
                    } else {
                        PopupMessage(result.message, function () {
                        });
                    }
                }
            });
        });
    };
}

function InitializeMandatorySymbol() {
    // setting * to red *
    $('.control-label').each(function () {
        if ($('#' + $(this).attr('for')).attr('data-val-required') != undefined) {
            if ($(this).text($(this).text().replace('*', ' ')).next().html() != ' *')
                $(this).text($(this).text().replace('*', ' ')).after('<span style="color: red;"> *</span>');
        }
    });

    $('form > div > ul > li').click(function () {
        //$('[data-val-required="' + $(this).html() + '"]').focus();
        if ($('[name="' + $(this).attr('key') + '"]').hasClass('ckeditor')) jQuery('.cke_wysiwyg_frame').contents().find('body').focus();
        else
            $('[name="' + $(this).attr('key') + '"]').focus();
    });

    $('form > div > div > ul > li').click(function () {
        //$('[data-val-required="' + $(this).html() + '"]').focus();
        if ($('[name="' + $(this).attr('key') + '"]').hasClass('ckeditor')) jQuery('.cke_wysiwyg_frame').contents().find('body').focus();
        else
            $('[name="' + $(this).attr('key') + '"]').focus();
    });
}

function GetMonthName(monthNumber) {
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'Mei', 'Jun', 'Jul', 'Agu', 'Sep', 'Okt', 'Nov', 'Des'];
    return months[monthNumber - 1];
}

$(document).ready(function () {
    $('div[class="data-result"]').find('table').DataTable({
        responsive: true,
        columnDefs: [{
            "targets": 'no-sort',
            "orderable": false
        }]
    });

    $('#btn-logout').click(function () {
        $.ajax({
            url: $(this).attr('data-href'),
            dataType: 'json',
            type: 'post',
            success: function (result) {
                window.location.href = result;
            }
        });
    });

    $('.date-picker').datepicker({
        //options
        dateFormat: 'dd M yy',
        changeMonth: true,
        changeYear: true,
        //...
    });

    $('select').attr('data-live-search', true);

    $('button[class*="addnew"]').click(function () {
        window.location.href = $(this).attr('data-href');
    })

    $('button[class*="cancel"]').click(function () {
        window.location.href = $(this).attr('data-href');
    })

    $('button[class*="search"]').click(function () {
        var $form = $(this).closest('form');
        var container = $(this).closest('.panel-body');
        //var propertyOrder = $(this).attr('data-property-order');
        //var pageSize = $(this).attr('data-pagesize');

        //var str = $(this).attr('data-hidden');
        //var temp = new Array();
        //// this will return an array with strings "1", "2", etc.
        //temp = str.split(",");

        //var skip = $(this).attr('data-skip');
        //var tempSkip = new Array();
        //tempSkip = skip.split(",");

        $.ajax({
            url: $form.attr('action'),
            data: $form.serialize(),
            dataType: 'html',
            type: 'post',
            beforeSend: function () {
                $form.showloading();
            },
            complete: function () {
                $form.hideloading();
            },
            success: function (dataResult) {
                $('.data-result', container).html(dataResult);
                //$('.table>thead>tr>th', container).attr("style", "cursor: pointer;");
                //$('tbody', container).on("click", "tr", function () {
                //    if ($(this).attr('data-edit') != undefined)
                //        window.location.href = $(this).attr('data-edit');
                //    else
                //        return;
                //});

                $('.data-result', container).find('table').DataTable({
                    responsive: true,
                    columnDefs: [{
                        "targets": 'no-sort',
                        "orderable": false,
                    }]
                });

                //$('.data-result', container).find('table').on('click', 'a.row-edit', function () {
                //    alert($(this).attr('data-id'));
                //});

                //$('.data-result', container).find('table').on('click', 'a.row-select', function () {
                //    alert($(this).attr('data-id'));
                //});

                //$('.data-result', container).find('table').on('click', 'a.row-delete', function () {
                //    alert($(this).attr('data-id'));
                //});

                //$('.data-result', container).find('table').on('click', 'a.row-custom1', function () {
                //    alert($(this).attr('data-id'));
                //});

                //$('.data-result', container).find('table').on('click', 'a.row-custom2', function () {
                //    alert($(this).attr('data-id'));
                //});

                //$('.data-result', container).find('table').removeAttr('style');

                //$('.current-page', container).val(1);

                //var isDescending = true;
                //$('.data-result', container).on("click", "li.page", function (e) {
                //    var page = $(this).attr('link');
                //    var objSearch = JSON.stringify($form.serializeObject());
                //    $('.current-page', container).val(page);
                //    pageSize = $('.data-result', container).find("#pagesize option:selected").val();
                //    GetDataTableJson(objSearch, page, pageSize, propertyOrder, isDescending);
                //});

                //$('.data-result', container).on("change", "#pagesize", function (e) {
                //    var page = 1;
                //    var objSearch = JSON.stringify($form.serializeObject());
                //    $('.current-page', container).val(page);
                //    pageSize = $('.data-result', container).find("#pagesize option:selected").val();
                //    GetDataTableJson(objSearch, page, pageSize, propertyOrder, isDescending);
                //});

                //$('.table>thead>tr>th', container).click(function (e) {
                //    var objSearch = JSON.stringify($form.serializeObject());
                //    var page = $('.current-page', container).val();
                //    propertyOrder = $(this).attr('data-id');
                //    isDescending = $(this).attr('data-desc');
                //    $('.table>thead>tr>th>span', container).remove();
                //    if (isDescending == "True") {
                //        $(this).attr('data-desc', 'False');
                //        $(this).html($(this).text() + '<span class="fa fa-caret-up pull-right"></span>');
                //    }
                //    else {
                //        $(this).attr('data-desc', 'True');
                //        $(this).html($(this).text() + '<span style="padding-top:5px" class="fa fa-caret-down pull-right"></span>');
                //    }
                //    GetDataTableJson(objSearch, page, pageSize, propertyOrder, isDescending);
                //});

                //function GetDataTableJson(objSearch, page, pageSize, propertyOrder, isDescending) {
                //    $.ajax({
                //        url: $form.attr('action') + 'Json',
                //        data: {
                //            stringSearch: objSearch,
                //            page: page,
                //            pageSize: pageSize,
                //            propertyOrder: propertyOrder,
                //            isDescending: isDescending
                //        },
                //        dataType: 'JSON',
                //        type: 'post',
                //        beforeSend: function () {
                //            $('.data-result', container).showloading();
                //        },
                //        complete: function () {
                //            $('.data-result', container).hideloading();
                //        },
                //        success: function (dataJson) {
                //            var table_row = '';
                //            $.each(dataJson.DataTable, function (i, value) {
                //                table_row += '<tr>';
                //                var styleDisplay = '';
                //                for (var key in value) {

                //                    if (tempSkip.any(function (s) { return s == key })) {
                //                        continue;
                //                    }

                //                    var styleDisplay = '';
                //                    if (temp.any(function (t) { return t == key })) {
                //                        styleDisplay = ' style="display:none;"';
                //                    }
                //                    if (key.indexOf("Date") >= 0) {
                //                        var date = new Date(parseInt(value[key].substr(6)));
                //                        table_row += '<td' + styleDisplay + '>' + date.getDate() + " " + GetMonthName(parseInt(date.getMonth()) + 1) + " " + date.getFullYear() + '</td>';
                //                    } else {
                //                        table_row += '<td' + styleDisplay + '>' + value[key].toString() + '</td>';
                //                    }
                //                }
                //                table_row += '</tr>';
                //            });
                //            $('tbody', container).html(table_row);
                //            $('.dataTables_paginate', container).html(dataJson.Pagination);
                //        }
                //    });
                //}
            }
        });

    });

    $('button[class*="save"]').Confirmation(function (href, $form, successmsg, failedmsg) {

        $.ajax({
            url: href,
            data: $form.serialize(),
            dataType: 'json',
            type: 'post',
            beforeSend: function () {
                $form.showloading();
            },
            complete: function () {
                $form.hideloading();
            },
            success: function (result) {
                if (result.Status == true) {
                    if (successmsg != undefined) {
                        PopupMessage(successmsg, function () {
                            if (result.Href != undefined)
                                window.location.href = result.Href;
                        });
                    } else {
                        PopupMessage(result.Message, function () {

                        });
                    }
                } else {
                    if (result.errorList != undefined) {
                        var errorResult = '';
                        errorResult += "<div class='validation-summary-errors' data-valmsg-summary='true'><ul>";
                        $.each(result.errorList, function (i, value) {
                            errorResult += "<li style='cursor: pointer; color: red; list-style-type: square; margin-left: 15px;' key='" + value.Key + "'>" + value.ErrorMessage + "</li>";
                        });
                        errorResult += "</ul></div>";

                        $('#ErrorSummary', $form).html(errorResult);
                        $(window).showErrorModelState();
                    }
                    else {
                        if (failedmsg != undefined) {
                            PopupMessage(failedmsg);
                        } else {
                            PopupMessage(result.Message);
                        }                        
                    }
                }
            }
        });

    });

    $('span[id*="popup"]').click(function () {
        var target = $(this).attr('data-target');
        var urltarget = $(this).attr('data-href');

        $.ajax({
            url: urltarget,
            dataType: 'html',
            success: function (data) {
                $(target).html(data);

                $('.date-picker', $(target)).datepicker({
                    //options
                    dateFormat: 'dd M yy',
                    changeMonth: true,
                    changeYear: true,
                    //...
                });

                $('.lookup-result', $(target)).find('table').DataTable({
                    responsive: true,
                    columnDefs: [{
                        "targets": 'no-sort',
                        "orderable": false,
                    }]
                });

                //event search
                $(target).find('button[class*="search"]').click(function () {
                    var $form = $(this).closest('form');
                    var container = $(this).closest('.modal-body');
                    //var propertyOrder = $(this).attr('data-property-order');
                    //var pageSize = $(this).attr('data-pagesize');

                    //var str = $(this).attr('data-hidden');
                    //var temp = new Array();
                    //// this will return an array with strings "1", "2", etc.
                    //temp = str.split(",");

                    //var skip = $(this).attr('data-skip');
                    //var tempSkip = new Array();
                    //tempSkip = skip.split(",");

                    $.ajax({
                        url: $form.attr('action'),
                        data: $form.serialize(),
                        dataType: 'html',
                        type: 'post',
                        beforeSend: function () {
                            $form.showloading();
                        },
                        complete: function () {
                            $form.hideloading();
                        },
                        success: function (dataResult) {
                            $('.lookup-result', container).html(dataResult);
                            //$('.table>thead>tr>th', container).attr("style", "cursor: pointer;");
                            //$('tbody', container).on("click", "tr", function () {
                            //    if ($(this).attr('data-edit') != undefined)
                            //        window.location.href = $(this).attr('data-edit');
                            //    else
                            //        return;
                            //});

                            $('.lookup-result', container).find('table').DataTable({
                                responsive: true,
                                columnDefs: [{
                                    "targets": 'no-sort',
                                    "orderable": false,
                                }]
                            });

                            //$('.data-result', container).find('table').removeAttr('style');

                            //$('.current-page', container).val(1);

                            //var isDescending = true;
                            //$('.lookup-result', container).on("click", "li.page", function (e) {
                            //    var page = $(this).attr('link');
                            //    var objSearch = JSON.stringify($form.serializeObject());
                            //    $('.current-page', container).val(page);
                            //    pageSize = $('.lookup-result', container).find("#pagesize option:selected").val();
                            //    GetDataTableLookupJson(objSearch, page, pageSize, propertyOrder, isDescending);
                            //});

                            //$('.lookup-result', container).on("change", "#pagesize", function (e) {
                            //    var page = 1;
                            //    var objSearch = JSON.stringify($form.serializeObject());
                            //    $('.current-page', container).val(page);
                            //    pageSize = $('.lookup-result', container).find("#pagesize option:selected").val();
                            //    GetDataTableLookupJson(objSearch, page, pageSize, propertyOrder, isDescending);
                            //});

                            //$('.table>thead>tr>th', container).click(function (e) {
                            //    var objSearch = JSON.stringify($form.serializeObject());
                            //    var page = $('.current-page', container).val();
                            //    propertyOrder = $(this).attr('data-id');
                            //    isDescending = $(this).attr('data-desc');
                            //    $('.table>thead>tr>th>span', container).remove();
                            //    if (isDescending == "True") {
                            //        $(this).attr('data-desc', 'False');
                            //        $(this).html($(this).text() + '<span class="fa fa-caret-up pull-right"></span>');
                            //    }
                            //    else {
                            //        $(this).attr('data-desc', 'True');
                            //        $(this).html($(this).text() + '<span style="padding-top:5px" class="fa fa-caret-down pull-right"></span>');
                            //    }
                            //    GetDataTableLookupJson(objSearch, page, pageSize, propertyOrder, isDescending);
                            //});

                            //function GetDataTableLookupJson(objSearch, page, pageSize, propertyOrder, isDescending) {
                            //    $.ajax({
                            //        url: $form.attr('action') + 'Json',
                            //        data: {
                            //            stringSearch: objSearch,
                            //            page: page,
                            //            pageSize: pageSize,
                            //            propertyOrder: propertyOrder,
                            //            isDescending: isDescending
                            //        },
                            //        dataType: 'JSON',
                            //        type: 'post',
                            //        beforeSend: function () {
                            //            $('.lookup-result', container).showloading();
                            //        },
                            //        complete: function () {
                            //            $('.lookup-result', container).hideloading();
                            //        },
                            //        success: function (dataJson) {
                            //            var table_row = '';
                            //            $.each(dataJson.DataTable, function (i, value) {
                            //                table_row += '<tr>';
                            //                var styleDisplay = '';
                            //                for (var key in value) {

                            //                    if (tempSkip.any(function (s) { return s == key })) {
                            //                        continue;
                            //                    }

                            //                    var styleDisplay = '';
                            //                    if (temp.any(function (t) { return t == key })) {
                            //                        styleDisplay = ' style="display:none;"';
                            //                    }
                            //                    if (key.indexOf("Date") >= 0) {
                            //                        var date = new Date(parseInt(value[key].substr(6)));
                            //                        table_row += '<td' + styleDisplay + '>' + date.getDate() + " " + GetMonthName(parseInt(date.getMonth()) + 1) + " " + date.getFullYear() + '</td>';
                            //                    } else {
                            //                        table_row += '<td' + styleDisplay + '>' + value[key].toString() + '</td>';
                            //                    }
                            //                }
                            //                table_row += '</tr>';
                            //            });
                            //            $('tbody', container).html(table_row);
                            //            $('.dataTables_paginate', container).html(dataJson.Pagination);
                            //        }
                            //    });
                            //}
                        }
                    });
                });
            }
        });
    });

    //$('button[class*="new"]').click(function () {
    //});
});

function enableButtons() {
    var buttons = document.getElementsByTagName("button");
    for (var i = 0; i < buttons.length; i++) {
        buttons.disabled = false;
    }
}
function disableButtons() {
    var buttons = document.getElementsByTagName("button");
    for (var i = 0; i < buttons.length; i++) {
        buttons.disabled = true;
    }
}

function GetForgeryHeader() {
    var token = $('[name=__RequestVerificationToken]').val();
    var headersSetup = {};
    headersSetup["__RequestVerificationToken"] = token;
    return headersSetup;
}

/*Region Ajax Extension and Customization*/
$.ajaxSetup({
    beforeSend: function () {
        disableButtons();
    },
    headers: GetForgeryHeader(),
    statusCode: {
        203: function () {
            window.location.href = $('#AccessDeniedURL').val();
        }
    },
    error: function (xhr, ajaxOptions, thrownError) {
        console.log(xhr.status);
        console.log(thrownError);
    },
    //error: function () {
    //    window.location.href = $('#ErrorURL').val();
    //},
    complete: function () {
        enableButtons();
    }
});

(function ($) {
    $.getAntiForgeryToken = function (tokenWindow, appPath) {
        // HtmlHelper.AntiForgeryToken() must be invoked to print the token.
        tokenWindow = tokenWindow && typeof tokenWindow === typeof window ? tokenWindow : window;

        appPath = appPath && typeof appPath === "string" ? "_" + appPath.toString() : "";
        // The name attribute is either __RequestVerificationToken,
        // or __RequestVerificationToken_{appPath}.
        var tokenName = "__RequestVerificationToken" + appPath;

        // Finds the <input type="hidden" name={tokenName} value="..." /> from the specified window.
        // var inputElements = tokenWindow.$("input[type='hidden'][name=' + tokenName + "']");
        var inputElements = tokenWindow.document.getElementsByTagName("input");
        for (var i = 0; i < inputElements.length; i++) {
            var inputElement = inputElements[i];
            if (inputElement.type === "hidden" && inputElement.name === tokenName) {
                return {
                    name: tokenName,
                    value: inputElement.value
                };
            }
        }
        return null;
    };

    $.appendAntiForgeryToken = function (data, token) {
        // Converts data if not already a string.
        if (data && typeof data !== "string") {
            data = $.param(data);
        }

        // Gets token from current window by default.
        token = token ? token : $.getAntiForgeryToken(); // $.getAntiForgeryToken(window).

        data = data ? data + "&" : "";
        // If token exists, appends {token.name}={token.value} to data.
        return token ? data + encodeURIComponent(token.name) + "=" + encodeURIComponent(token.value) : data;
    };

    // Wraps $.post(url, data, callback, type) for most common scenarios.
    $.postAntiForgery = function (url, data, callback, type) {
        return $.post(url, $.appendAntiForgeryToken(data), callback, type);
    };

    // Wraps $.ajax(settings).
    $.ajaxAntiForgery = function (settings) {
        // Supports more options than $.ajax(): 
        // settings.token, settings.tokenWindow, settings.appPath.
        var token = settings.token ? settings.token : $.getAntiForgeryToken(settings.tokenWindow, settings.appPath);
        settings.data = $.appendAntiForgeryToken(settings.data, token);
        return $.ajax(settings);
    };
})(jQuery);