# FeedHenry C#/.NET SDK

[![Build status](https://ci.appveyor.com/api/projects/status/iio3m7a9yhiu0xfr?svg=true)](https://ci.appveyor.com/project/edewit/fh-dotnet-sdk-tekce)

FeedHenry native .NET SDK for Windows Phone 8+ and Xamarin platform. It supports Portable Class Library (PCL) projects as well.

## Install

You can install the SDK to your project either automatically (using NuGet) or manually.

### NuGet (Recommended)

FH SDK is available on NuGet: https://www.nuget.org/packages/FH.SDK/.
If you are using the NuGet plugin on Visual Studio or Xamarin Studio, please search for FH.SDK.
NuGet will install dependecy libraries automatically.

### Manually

The .dll assembly files can be found in the [Distribution directory](Dist). Please use the .dll files inside the folder that is corresponding to your project's target platform.
The SDK is depending on [Json.Net](https://www.nuget.org/packages/Newtonsoft.Json/) and [Microsoft HTTP Client Libraries](https://www.nuget.org/packages/Microsoft.Net.Http/). You need to install the assemblies of those libraries as well if they are not available in your project.

## Usage

### FHClient.Init

Initialise the SDK. Normally it should be called immediately after the app finish initialising.
FHClient is available in the following namespaces:

* FHSDK.Phone - For WP8
* FHSDK.Droid - For Android
* FHSDK.Touch - For iOS

Depending on your app's build target, only one of these name spaces should be available to your app.

The main reason for having the same FHClient class defined in different name spaces is to ensure that the platform-specific assembly file is loaded correctly.

````cs
try {
  bool inited = await FHClient.Init();
  if(inited) {
    //Initialisation is successful
  }
}
catch(FHException e) {
  //Initialisation failed, handle exception
}
````

### FH.Cloud

Invoke a cloud function. Can be used in PCL projects.

````cs
FHResponse response = await FH.Cloud("api/echo", "GET", null, null);
if(null == response.Error)
{
  //no error occured, the request is successful
  string rawResponseData = response.RawResponse;
  //you can get it as JSONObject (require Json.Net library)
  JObject resJson = response.GetResponseAsJObject();
  //process response data
}
else
{
  //error occured during the request, deal with it.
  //More infomation can be access from response.Error.InnerException
}
````

### FH.Auth

Call the FeedHenry Authentication API with the given policyId. This is normally used for OAuth type authentications. The user will be prompted for login details and the login result will be returned. Can be used in PCL projects.

````cs
string authPolicy = "TestGooglePolicy";
FHResponse res = await FH.Auth(authPolicy);
if (null == res.Error)
{
    //auth is successful
}
else
{
    //auth failed
}
````

### FH.GetCloudHost

Return the url of the cloud host the app is communicating with. Can be used in PCL projects.

### FH.Act (Deprecated)

Invoke a cloud function which you have defined in cloud/main.js (the old way). Can be used in PCL projects.

For full list of APIs, please check [FH .NET SDK API References](http://feedhenry.org/fh-dotnet-sdk/Documentations/html/index.html).

## Sync Client

See [Sync Client Usage Guide](SyncClient.md).

## Solution Structure

The complete FH .NET SDK contains four projects:

* FHSDK

  A PCL library contains core code. It is required by all the other three projects.  Most of the APIs are implemented here. It contains a few interface definitions which are implemented in each platform's project.  Another important function of this library is to automatically figure out the correct implementions for the inferfaces when running on devices (see code in [FHSDK/Adaptation](FHSDK/Adaptation)). The assembly file built by the project can be used by other PCL projects.

* FHSDKPhone

  A WP8 library project. Contains implementaion details for WP8 platform.

* FHXamarinAndroidSDK

  A Xamarin Android library project. Contains implementaion details for Android platform.

* FHXamarinIOSSDK

  A Xamarin IOS library project. Contains implementaion details for iOS platform.

## Testing

There are 3 test projects ceated for each platform:

* FHSDKAndroidTest (NUnit Lite for Android)
* FHSDKIOSTest (NUnit Lite for IOS)
* FHSDKWindowsPhoneTestNative (WindowsPhoneUnitTest)

They all link to the test files in [FHSDKTestShared](FHSDKTestShared) project.

Before running the test, make sure the [TestCloudApp](https://github.com/fheng/fh-sdks-test-cloud-app) is running somewhere, and update the following files to point them to the cloud app:

* [fhconfig.local.properties](FHSDKAndroidTest/Assets/fhconfig.local.properties)
* [fhconfig.local.plist](FHSDKIOSTest/fhconfiglocal.plist)
* [fhconfig.local.json](FHSDKWindowsPhoneTestNative/fhconfig.local.json)

Then you can deploy the test projects to the emulators or devices and run the tests (For Windows Phone, just run the tests using Visual Studio).

## Building

Open `FHSDK.sln` solution in Visual Studio and build.
For Xamarin open `FHSDK-xamarin-studio.sln` in root folder.

### Documentation
To generate and publish the API docs, please do the following:

* [Install doxygen](http://www.stack.nl/~dimitri/doxygen/download.html)
* From the command prompt run `doxygen .\Documentations\Doxyfile`
* Commit changes
* Checkout `gh-pages` branch
* Merge the branch which contains the latests docs
* Push `gh-pages` to remote repo

## Publishing to NuGet

Read the [infomation in the dist folder](Dist/make-dist.md)
