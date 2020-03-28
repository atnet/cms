#!/usr/bin/env sh

echo "======================================="
echo "= JR Cms .NET ! packer ="
echo "======================================="


RELEASE_DIR=$(pwd)/out/aspnet

echo "setup1: prepare.." && \
    rm -rf out && mkdir -p ${RELEASE_DIR} && cd src/JR.Cms.AspNet.App && sh copy_assets.sh 


echo "setup2: buiding.." && \
msbuild *.csproj /p:Configuration=Release && \
    cp -r bin public templates oem install plugins root ${RELEASE_DIR} && \
    cp  Global.asax Web.config ${RELEASE_DIR}

cd ${RELEASE_DIR} && \
    sed -i 's/compilation debug="true"/compilation debug="false"/g' Web.config && \
    echo "setup3: clean assemblies.." && \
    cd bin && rm -rf *.pdb *.xml \
    Microsoft.Extensions.DependencyInjection.Abstractions.dll \
    Google.Protobuf.dll Microsoft.DotNet.PlatformAbstractions.dll \
    Microsoft.Extensions.WebEncoders.dll Microsoft.Extensions.Options.dll \
    Microsoft.Extensions.ObjectPool.dll Microsoft.Extensions.Localization* \
    Microsoft.Extensions.FileProviders.Abstractions.dll Microsoft.Extensions.Dependency* \
    Microsoft.Extensions.Configuration.Abstractions.dll Microsoft.Extensions.Caching* \
    Microsoft.Win32.Registry.dll Microsoft.Web.Infrastructure.dll Microsoft.Net.Http.Headers.dll \
    Microsoft.AspNetCore.WebUtilities.dll \
    Microsoft.AspNetCore.JsonPatch.dll Microsoft.AspNetCore.Http.Extensions.dll \
    Microsoft.AspNetCore.Http.dll Microsoft.AspNetCore.Http.Abstractions.dll \
    Microsoft.AspNetCore.Html.Abstractions.dll Microsoft.AspNetCore.Hosting* \
    Microsoft.AspNetCore.A* Microsoft.AspNetCore.C* Microsoft.AspNetCore.D* \
    Microsoft.AspNetCore.Mvc* Microsoft.AspNetCore.M* Microsoft.AspNetCore.R* 

echo 'setup4: upgrade dll..' &&  cp ../../../dll/aspnet/* . &&  cd ..
    
echo 'setup5: packing..' && \
    cp ../../LICENSE ../../README.md . && \
    tar czf ../../jrcms-aspnet-latest.tar.gz *
    
echo "package finished!"
    

    