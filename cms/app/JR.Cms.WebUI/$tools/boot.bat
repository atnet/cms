::����˿�
set port=80

::������(����������Ҫ�趨)
set host=localhost

::������ַ
set proxy=http://www.j6.cc


cd ../
start /b $tools\server-proxy.exe -host %host% -port %port% -proxy %proxy%