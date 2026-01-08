
var Cart = {
    processingRequest: false,
    headercartselector: '',

    init: function (headercartselector) {
        this.processingRequest = false;
        this.headercartselector = headercartselector;
    },
    // Add product to cart the cart from the catalog page.
    addProductToCart_catalog: function (addUrl) {
        if (this.processingRequest !== false) {
            return;
        }
        this.setProcessingRequest(true);

        // TODO: Handle anti forgery token
        var postData = {}

        this.sendAjaxRequest(addUrl, postData);
    },
    setProcessingRequest: function (display) {
        displayAjaxLoading(display);
        this.processingRequest = display;
    },
    sendAjaxRequest: function (requestUrl, postData) {
        $.ajax({
            cache: false,
            url: requestUrl,
            type: "POST",
            data: postData,
            success: this.processingSuccess,
            complete: this.resetProcessing,
            error: this.requestError
        });
    },
    processingSuccess: function (response) {
        // TODO: Update cart section
        if (response.updateheadercarthtml) {
            $(Cart.headercartselector).html(response.updateheadercarthtml)
        }

        // TODO: Display notification (success or failure)

        if (response.redirect) {
            location.href = response.redirect;
            return true;
        }
    },
    resetProcessing: function () {
        Cart.setProcessingRequest(false);
    },
    requestError: function () {
        alert("Error adding a product to the cart")
    }
}