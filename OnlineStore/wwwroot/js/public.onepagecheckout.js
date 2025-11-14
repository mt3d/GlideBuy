
var Checkout = {

    // TODO: Complete
    ajaxFailure: function () {

    }
}

var Billing = {
    form: false,
    saveUrl: '',
    disableBillingAddressCheckoutStep: false,
    guest: false,

    init: function (form, saveUrl, disableBillingAddressCheckoutStep, guest) {
        this.form = form;
        this.saveUrl = saveUrl;
        this.disableBillingAddressCheckoutStep = disableBillingAddressCheckoutStep;
        this.guest = guest;
    },
    
    save: function () {

        $.ajax({
            cache: false,
            url: this.saveUrl,
            data: $(this.form).serialize(),
            type: 'POST',
            success: this.nextStep,
            complete: this.resetLoadWaiting,
            error: Checkout.ajaxFailure
        });
    }
}