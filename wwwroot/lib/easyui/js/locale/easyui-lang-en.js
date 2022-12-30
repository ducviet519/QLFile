if ($.fn.pagination){
	$.fn.pagination.defaults.beforePageText = 'Trang';
	$.fn.pagination.defaults.afterPageText = 'of {pages}';
	$.fn.pagination.defaults.displayMsg = 'Đang xem {from} đến {to} trong tổng số {total} mục';
}
if ($.fn.datagrid){
	$.fn.datagrid.defaults.loadMsg = 'Đang tải dữ liệu, xin hãy đợi ...';
}
if ($.fn.treegrid && $.fn.datagrid){
	$.fn.treegrid.defaults.loadMsg = $.fn.datagrid.defaults.loadMsg;
}
if ($.messager){
	$.messager.defaults.ok = 'Ok';
	$.messager.defaults.cancel = 'Cancel';
}
$.map(['validatebox','textbox','passwordbox','filebox','searchbox',
		'combo','combobox','combogrid','combotree',
		'datebox','datetimebox','numberbox',
		'spinner','numberspinner','timespinner','datetimespinner'], function(plugin){
	if ($.fn[plugin]){
		$.fn[plugin].defaults.missingMessage = 'Trường này không được bỏ trống.';
	}
});
if ($.fn.validatebox){
	$.fn.validatebox.defaults.rules.email.message = 'Xin hãy nhập đúng địa chỉ Email.';
	$.fn.validatebox.defaults.rules.url.message = 'Xin hãy nhập đúng URL.';
	$.fn.validatebox.defaults.rules.length.message = 'Xin hãy nhập giá trị trong khoảng từ {0} đến {1}.';
	$.fn.validatebox.defaults.rules.remote.message = 'Xin hãy kiểm tra lại dữ liệu.';
}
if ($.fn.calendar) {
	$.fn.calendar.defaults.weeks = ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'];
	$.fn.calendar.defaults.months = ['Th1', 'Th2', 'Th3', 'Th4', 'Th5', 'Th6', 'Th7', 'Th8', 'Th9', 'Th10', 'Th11', 'Th12'];
}
if ($.fn.datebox){
	$.fn.datebox.defaults.currentText = 'Hôm nay';
	$.fn.datebox.defaults.closeText = 'Đóng';
	$.fn.datebox.defaults.okText = 'Ok';
}
if ($.fn.datetimebox && $.fn.datebox){
	$.extend($.fn.datetimebox.defaults,{
		currentText: $.fn.datebox.defaults.currentText,
		closeText: $.fn.datebox.defaults.closeText,
		okText: $.fn.datebox.defaults.okText
	});
}
