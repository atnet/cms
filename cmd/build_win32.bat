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

echo  /keyfile:%dir%\src\JR.Cms.Core\jr.cms.snk>nul

"%cur%/merge.exe" /closed /ndebug /targetplatform:v4 /target:dll /out:%dir%/dist\jrcms.dll^
 JR.Cms.Core.dll JR.Cms.BLL.dll JR.Cms.DAL.dll JR.Cms.Domain.Interface.dll^
 JR.Cms.CacheService.dll JR.Cms.DataTransfer.dll JR.Cms.Domain.Implement.Content.dll^
 JR.Cms.DB.dll JR.Cms.Cache.dll JR.Cms.Domain.Implement.Site.dll JR.Cms.Domain.Implement.User.dll JR.Cms.Infrastructure.dll ^
 JR.Cms.Service.dll JR.Cms.ServiceContract.dll^
 JR.Cms.ServiceUtil.dll JR.Cms.ServiceRepository.dll JR.Cms.IDAL.dll^
 JR.Cms.Sql.dll JR.Cms.Utility.dll JR.Cms.WebImpl.dll


  echo ���!�����:%dir%/dist/jrcms.dll

)


pause