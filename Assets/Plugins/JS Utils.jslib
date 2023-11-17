var plugin = {
	OpenTab : function(url)
	{
		url = Pointer_stringify(url);
		window.open(url,'_blank');
	},
	GoBack: function () {
        	 window.history.back();
    	},
};
mergeInto(LibraryManager.library, plugin);