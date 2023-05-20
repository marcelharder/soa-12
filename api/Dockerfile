
FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY Release App/
WORKDIR /App

COPY conf/*.xml /App/conf/
COPY assets/pdf/*.pdf /App/assets/pdf/
COPY assets/images/*.jpg /App/assets/images/
COPY assets/images/*.jpeg /App/assets/images/

#COPY ./wait-for-it.sh /App/wait-for-it.sh
#RUN chmod +x /App/wait-for-it.sh

ENTRYPOINT [ "dotnet", "api.dll" ]
