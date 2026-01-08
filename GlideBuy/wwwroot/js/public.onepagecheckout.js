
var Checkout = {

    // TODO: Complete
    ajaxFailure: function () {

    },

    setStepResponse: function (response) {

        if (response.goto_section) {
            Checkout.gotoSection(response.goto_section);
            console.log(response.goto_section);
            return true;
        }
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
        console.log("Saving...");

        $.ajax({
            cache: false,
            url: this.saveUrl,
            data: $(this.form).serialize(),
            type: 'POST',
            success: this.nextStep,
            complete: this.resetLoadWaiting,
            error: Checkout.ajaxFailure
        });
    },

    nextStep: function (response) {
        if (response.error) {
            if (typeof response.message === 'string') {
                alert(response.message);
            } else {
                alert(response.message.join("\n"));
            }

            return false;
        }

        Checkout.setStepResponse(response);
    }
}