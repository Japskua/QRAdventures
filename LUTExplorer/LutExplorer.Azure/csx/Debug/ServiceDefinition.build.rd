<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="LutExplorer.Azure" generation="1" functional="0" release="0" Id="e76b5a83-cf4e-449e-8cdd-c219fd8abe65" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="LutExplorer.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="LutExplorer:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/LutExplorer.Azure/LutExplorer.AzureGroup/LB:LutExplorer:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="LutExplorer:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/LutExplorer.Azure/LutExplorer.AzureGroup/MapLutExplorer:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="LutExplorer:StorageConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/LutExplorer.Azure/LutExplorer.AzureGroup/MapLutExplorer:StorageConnectionString" />
          </maps>
        </aCS>
        <aCS name="LutExplorerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/LutExplorer.Azure/LutExplorer.AzureGroup/MapLutExplorerInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:LutExplorer:Endpoint1">
          <toPorts>
            <inPortMoniker name="/LutExplorer.Azure/LutExplorer.AzureGroup/LutExplorer/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapLutExplorer:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/LutExplorer.Azure/LutExplorer.AzureGroup/LutExplorer/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapLutExplorer:StorageConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/LutExplorer.Azure/LutExplorer.AzureGroup/LutExplorer/StorageConnectionString" />
          </setting>
        </map>
        <map name="MapLutExplorerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/LutExplorer.Azure/LutExplorer.AzureGroup/LutExplorerInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="LutExplorer" generation="1" functional="0" release="0" software="C:\Projects\QRAdventures\LUTExplorer\LutExplorer.Azure\csx\Debug\roles\LutExplorer" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="768" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="StorageConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;LutExplorer&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;LutExplorer&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/LutExplorer.Azure/LutExplorer.AzureGroup/LutExplorerInstances" />
            <sCSPolicyFaultDomainMoniker name="/LutExplorer.Azure/LutExplorer.AzureGroup/LutExplorerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="LutExplorerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="LutExplorerInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="7816fcdb-4662-4549-a674-ecfa7eb4182d" ref="Microsoft.RedDog.Contract\ServiceContract\LutExplorer.AzureContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="2bd9f1ad-e29f-49e2-8e3d-92a92386d6f0" ref="Microsoft.RedDog.Contract\Interface\LutExplorer:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/LutExplorer.Azure/LutExplorer.AzureGroup/LutExplorer:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>