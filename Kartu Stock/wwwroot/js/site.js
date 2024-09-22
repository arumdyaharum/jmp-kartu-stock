$(document).ready(function () {
    const dataTable = new DataTable('#transactionTable');

    $("#startDate").datepicker({
        dateFormat: "yy-mm-dd"
    });
    $("#endDate").datepicker({
        dateFormat: "yy-mm-dd"
    });

    $('#startDate, #endDate, #productId').on('change', () => {
        const startDate = $('#startDate').val();
        const endDate = $('#endDate').val();

        if (new Date(startDate) > new Date()) {
            $('#alertWarningTopMessage').text('Tanggal awal tidak boleh lebih maju daripada hari ini!');
            $('#alertWarningTop').show();
        } else if (new Date(endDate) > new Date()) {
            $('#alertWarningTopMessage').text('Tanggal akhir tidak boleh lebih maju daripada hari ini!');
            $('#alertWarningTop').show();
        } else if (new Date(startDate) > new Date(endDate)) {
            $('#alertWarningTopMessage').text('Tanggal awal tidak boleh lebih maju daripada tanggal akhir!');
            $('#alertWarningTop').show();
        } else {
            $('#alertWarningTop').hide();
        }

        dataTable.rows().remove().draw();
    });

    $('#searchForm').on('submit', function (event) {
        const startDate = $('#startDate').val();
        const endDate = $('#endDate').val();

        if (new Date(startDate) > new Date() || new Date(endDate) > new Date() || new Date(startDate) > new Date(endDate)) {
            event.preventDefault(); // Prevent form from submitting
        }
    });
});
