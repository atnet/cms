@echo off
color 66

echo =======================================
echo = JR Cms .NET ! ���ĳ������ɹ��� =
echo =======================================

set cur=%cd%
%~d0
cd ..\
echo �ϼ�Ŀ¼Ϊ��%cd%
set dir=%cd%

echo "set dir2=%~dp0 echo ��ǰĿ¼">nul
set megdir=%dir%\dist\bin\

if exist "%cur%/merge.exe" (

  echo ������,���Ե�...
  cd %dir%/src/bin/

echo  /keyfile:%dir%\src\t2.cms.snk>nul

"%cur%/merge.exe" /closed /ndebug /targetplatform:v4 /target:dll /out:%dir%/dist\jrcms.dll^
 T2.Cms.Core.dll T2.Cms.BLL.dll T2.Cms.DAL.dll T2.Cms.Domain.Interface.dll^
 T2.Cms.CacheService.dll T2.Cms.DataTransfer.dll T2.Cms.Domain.Implement.Content.dll^
 T2.Cms.DB.dll T2.Cms.Cache.dll T2.Cms.Domain.Implement.Site.dll T2.Cms.Domain.Implement.User.dll T2.Cms.Infrastructure.dll ^
 T2.Cms.Service.dll T2.Cms.ServiceContract.dll^
 T2.Cms.ServiceUtil.dll T2.Cms.ServiceRepository.dll T2.Cms.IDAL.dll^
 T2.Cms.Sql.dll T2.Cms.Utility.dll T2.Cms.WebImpl.dll


  echo ���!�����:%dir%/dist/jrcms.dll

)


pause