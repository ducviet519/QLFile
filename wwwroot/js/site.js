﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//PopUp cho <a>
$(function () {
    var ReportPopupElement = $('#ReportPopup');
    $('a[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            ReportPopupElement.html(data);
            ReportPopupElement.find('.modal').modal('show');
        });
    });

    ReportPopupElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var methodType = form.attr('method');
        $.ajax({
            type: methodType,
            url: actionUrl,
            data: form.serialize(),
            success: function (data) {
                alert("Thành công!");
            },
            error: function (xhr, desc, err) {
                alert("Lỗi!");
            }
        }).done(function (data) {
            ReportPopupElement.find('.modal').modal('hide');
            location.reload();
        })
    });   
});