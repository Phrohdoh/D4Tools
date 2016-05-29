all:
	xbuild /nologo /t:rebuild D4Tools.sln

clean:
	xbuild /nologo /t:clean
	find . -name "*.o" -exec rm {} \;
	find . -name "*.ppu" -exec rm {} \;
