// Taken from RTB.

function isInt(%string)
{
	%numbers = "0123456789";
	for(%i=0;%i<strLen(%string);%i++)
	{
		%char = getSubStr(%string,%i,1);
		if(strPos(%numbers,%char) $= -1)
			return 0;
	}
	return 1;
}