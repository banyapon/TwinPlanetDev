mergeInto(LibraryManager.library, 
{
	getToken: function () 
	{
		var text = new URLSearchParams(window.location.search).get("code")
		if (text != null)
		{
			var bufferSize = lengthBytesUTF8(text) + 1;
			var buffer = _malloc(bufferSize);
			stringToUTF8(text, buffer, bufferSize);
			return buffer;
		}
		text = "nocode";
		var bufferSize = lengthBytesUTF8(text) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(text, buffer, bufferSize);
		return buffer;;
	},
	
});