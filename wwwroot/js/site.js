// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$body = $("body");

$(document).on({
    ajaxStart: function () { $body.addClass("loading"); },
    ajaxStop: function () { $body.removeClass("loading"); }
});

function searchDataTable(id, columnData, url, disableColumn) {
    var array = [];
    $.each(disableColumn.split(','), function (idx, val) {
        array.push(parseInt(val));
    });
    if (disableColumn == '') { disableColumn = 0; }
    var table = $(id).DataTable();
    if ($.fn.dataTable.isDataTable(id)) {
        table.destroy();
        $(id).find('thead .filters').remove();
        $(id).find('tbody').empty();
    }
    var table = $(id).DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": true,
        "processing": true,
        "ordering": true,
        "info": true,
        "autoWidth": true,
        "responsive": false,
        "order": [[0, 'asc']],
        "columnDefs": [
            { orderable: false, targets: array },
            { orderable: false, targets: 'filters' },
            { className: "text-wrap", targets: "_all" },
            { defaultContent: '', targets: "_all" },
        ],
        "ajax": {
            "url": url,
            "type": "GET",
            "datatype": "json"
        },
        "columns": columnData,
        "language": {
            "sProcessing": "Đang tải dữ liệu...",
            "sLengthMenu": "Xem _MENU_ mục",
            "sZeroRecords": "Không tìm thấy dòng nào phù hợp",
            "sInfo": "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
            "sInfoEmpty": "Đang xem 0 đến 0 trong tổng số 0 mục",
            "sInfoFiltered": "(được lọc từ _MAX_ mục)",
            "sInfoPostFix": "",
            "sSearch": "Tìm:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Đầu",
                "sPrevious": "Trước",
                "sNext": "Tiếp",
                "sLast": "Cuối"
            }
        },
    });
    return table;
}

function searchDataTableWithInput(id, columnData, url, pageLength, disableColumn, disableInput) {
    var array = [];
    $.each(disableColumn.split(','), function (idx, val) {
        array.push(parseInt(val));
    });
    if (disableColumn == '') { disableColumn = 0; }
    var table = $(id).DataTable();
    if ($.fn.dataTable.isDataTable(id)) {
        table.destroy();
        $(id).find('thead .filters').remove();
        $(id).find('tbody').empty();
    }

    $(id +' thead tr')
        .clone(true)
        .addClass('filters')
        .appendTo(id +' thead');

    $(id).DataTable({
        "paging": true,
        "lengthChange": false,
        "pageLength": pageLength,
        "searching": true,
        "processing": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
        "orderCellsTop": true,
        "order": [[0, 'asc']],
        "columnDefs": [
            { orderable: false, targets: array },
            { className: "text-wrap", targets: "_all" },
            { defaultContent: '', targets: "_all" },
        ],
        "ajax": {
            "url": url,
            "type": "GET",
            "datatype": "json"
        },
        "columns": columnData,
        "initComplete": function () {
            var api = this.api();

            // For each column
            api
                .columns()
                .eq(0)
                .each(function (colIdx) {
                    // Set the header cell to contain the input element
                    if (colIdx > 0 && colIdx < 8) {
                        var cell = $('.filters th').eq(
                            $(api.column(colIdx).header()).index()
                        );
                        var title = $(cell).text();
                        $(cell).html('<input type="text" class="form-control col p-0 m-0" />');
                        //$(cell).html('<input type="text" class="form-control col p-0 m-0" placeholder="' + title + '" />');
                    }
                    else {
                        var cell = $('.filters th').eq(
                            $(api.column(colIdx).header()).index()
                        );
                        var title = $(cell).text();
                        $(cell).empty();
                        
                    }
                    // On every keypress in this input
                    $(
                        'input',
                        $('.filters th').eq($(api.column(colIdx).header()).index())
                    )
                        .off('keyup change')
                        .on('change', function (e) {
                            // Get the search value
                            $(this).attr('title', $(this).val());
                            var regexr = '({search})'; //$(this).parents('th').find('select').val();

                            var cursorPosition = this.selectionStart;
                            // Search the column for that value
                            api
                                .column(colIdx)
                                .search(
                                    this.value != ''
                                        ? regexr.replace('{search}', '(((' + this.value + ')))')
                                        : '',
                                    this.value != '',
                                    this.value == ''
                                )
                                .draw();
                        })
                        .on('keyup', function (e) {
                            e.stopPropagation();

                            $(this).trigger('change');
                            $(this)
                                .focus()[0]
                                .setSelectionRange(cursorPosition, cursorPosition);
                        });
                });
        },
        "language": {
            "sProcessing": "Đang tải dữ liệu...",
            "sLengthMenu": "Xem _MENU_ mục",
            "sZeroRecords": "Không tìm thấy dòng nào phù hợp",
            "sInfo": "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
            "sInfoEmpty": "Đang xem 0 đến 0 trong tổng số 0 mục",
            "sInfoFiltered": "(được lọc từ _MAX_ mục)",
            "sInfoPostFix": "",
            "sSearch": "Tìm:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Đầu",
                "sPrevious": "Trước",
                "sNext": "Tiếp",
                "sLast": "Cuối"
            }
        },
    });
}

$(function () {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
});

$.fn.clearData = function ($form) {
    $form.find('input:text, input:password, input:file, select, textarea').val('');
    $form.find('input:radio, input:checkbox')
        .removeAttr('checked').removeAttr('selected');
}

$.fn.callModal = function (url) {

    var ReportPopupElement = $('#ReportPopup'); 
    $.ajax({
        url: url,
        dataType: 'html',
        success: function (data) {
            $("body").find(".modal-backdrop").remove();
            ReportPopupElement.html(data);
            ReportPopupElement.find('.modal').modal('show');
        }, error: function (xhr, status) {
            switch (status) {
                case 404:
                    $(this).callToast("error", 'Lỗi!', 'Đường dẫn không đúng hoặc tính năng không tồn tại!');
                    break;
                case 500:
                    $(this).callToast("error", 'Lỗi!', 'Không kết nối được tới Server!');
                    break;
                case 0:
                    $(this).callToast("error", 'Lỗi!', 'Hệ thống không phản hồi!');
                    break;
                default:
                    $(this).callToast("error", 'Lỗi!', 'Sự cố không xác định! Lỗi: ' + status);
            }
        }
    });
}

$.fn.callToast = function (status, title, msg) {
    toastr.options = {
        "closeButton": false,
        "debug": true,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    if (status == "success") {
        toastr.success(msg, title)
    }
    else if (status == "info") {
        toastr.info(msg, title)
    }
    else if (status == "warning") {
        toastr.warning(msg, title)
    }
    else if (status == "error") {
        toastr.error(msg, title)
    }
}

$.fn.callDataTable = function (disableColumn, pageLength) {
    var array = [];
    $.each(disableColumn.split(','), function (idx, val) {
        array.push(parseInt(val));
    });
    if (disableColumn == '') { disableColumn = 0; }

    var table = $(this).DataTable({
        "paging": true,
        "lengthChange": false,
        "pageLength": pageLength,
        "searching": true,
        "processing": true,
        "ordering": true,
        "info": true,
        "autoWidth": true,
        "responsive": true,
        "order": [[0, 'asc']],
        "columnDefs": [
            { orderable: false, targets: array },
            { className: "text-wrap", targets: "_all" },
            { defaultContent: '', targets: "_all" },
        ],
        "language": {
            "sProcessing": "Đang tải dữ liệu...",
            "sLengthMenu": "Xem _MENU_ mục",
            "sZeroRecords": "Không tìm thấy dòng nào phù hợp",
            "sInfo": "Đang xem _START_ đến _END_ trong tổng số _TOTAL_ mục",
            "sInfoEmpty": "Đang xem 0 đến 0 trong tổng số 0 mục",
            "sInfoFiltered": "(được lọc từ _MAX_ mục)",
            "sInfoPostFix": "",
            "sSearch": "Tìm:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Đầu",
                "sPrevious": "Trước",
                "sNext": "Tiếp",
                "sLast": "Cuối"
            }
        },
    });
    return table;
}








