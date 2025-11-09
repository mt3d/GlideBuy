/**
 * The script manages a multi-section checkout interface where each “section”
 * (for example Billing, Shipping, Payment) can be expanded or collapsed like
 * panels in an accordion. Only one section is visible (“active”) at a time.
 * 
 * It ensures:
 * Only one section is open.
 * You can’t skip to the next section unless allowed.
 * Sections can be shown, hidden, opened, or closed programmatically.
 * The page’s URL updates to reflect which section is open (via the hash).
 * Other scripts can respond to events such as “section opened” or “section closed”.
 */

var Accordion = {
	checkAllowBeforeOpenning: false,
	sections: new Array(),
	disallowAccessToNextSections: false,
	currentSectionId: false,
	headers: new Array(),

	// elem and clickableEntity are just strings.
	init: function (elem, clickableEntity, checkAllowBeforeOpenning) {
		this.checkAllowBeforeOpenning = checkAllowBeforeOpenning || false;
		this.disallowAccessToNextSections = false;

		/* # is the id selector in css. This is written using jQuery.
		 * Find every element that has the class tab-section inside the element
		 * whose id is elmen.
		 * 
		 * The $ is simply a shortcut to the global jQuery object.
		 * https://api.jquery.com/jQuery/
		*/
		this.sections = $('#' + elem + ' .tab-section');

		// jQuery then returns a wrapped set, which is a jQuery object that behaves
		// like a list of DOM elements, but with many helper methods such as .on(),
		// .addClass(), .hide(), .show(), etc.
		// NOTE: Mind the spaces around .tab-section
		var headers = $('#' + elem + ' .tab-section ' + clickableEntity);
		//var headers = $('#' + elem + ' .tab-section ' + clickableEntity);

		headers.on('click', function () {
			Accordion.headerClicked($(this));
		});

		console.log(headers);
		console.log('init successful');
	},

	// Complete
	headerClicked: function (section) {
		// jQuery: Get the parent of each element in the current set of matched elements,
		// optionally filtered by a selector.
		Accordion.openSection(section.parent('.tab-section'));

		console.log('header was clicked');
	},

	openSection: function (section) {
		var section = $(section);

		if (this.checkAllowBeforeOpenning && !section.hasClass('allow')) {
			return;
		}

		if (section.attr('id') != this.currentSectionId) {
			var previousSectionId = this.currentSectionId;
			this.closeExistingSection();
			this.currentSectionId = section.attr('id');
			$('#' + this.currentSectionId).addClass('active');
			var contents = section.children('.a-item');
			$(contents[0]).show();
			location.hash = section.attr('id');
		}
	},

	closeSection: function (sectionId) {
		var section = $(sectionId);
		section.removeClass('active');

		var contents = section.children('.a-item');
		$(contents[0]).hide();

		console.log('Section is closed');
	},

	// Complete
	closeExistingSection: function () {
		if (this.currentSectionId) {
			this.closeSection($('#' + this.currentSectionId));
		}
	}
};