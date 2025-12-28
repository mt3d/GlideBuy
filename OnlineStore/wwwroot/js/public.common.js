function displayAjaxLoading(display) {
    if (display) {
        $('#loading-block-window').show();
    }
    else {
        $('#loading-block-window').hide('slow');
    }
}