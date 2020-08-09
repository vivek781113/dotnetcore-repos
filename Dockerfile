#1.Specify image
#For content, the first thing we need to define is an image we want to base it on. We also need to set a working directory where we want the files to end up on the container. We do that with the command FROM and WORKDIR, like so:
#What we are saying here is to go grab an image with a small OS image made for .Net Core. We also say that our working directory is /app.
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as build-env
WORKDIR /app

#2.Copy project file
#Next, we need to copy the project file ending in .csproj. Additionally, we also need to call dotnet restore, to ensure we install all specified dependencies, like so:
#<src-actual pc> <dest container>
COPY *.csproj ./
RUN dotnet-restore

#3.Copy and Build
#Next, we need to copy our app files and build our app, like so:
COPY . ./
RUN dotnet publish -c Release -o out

#4.Build runtime image
#Here we again specify our image and our working directory, like so:
#There is a difference though, this time we want to copy our built files to app/out
#Starting the app
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "WebAPI3_1.dll"]