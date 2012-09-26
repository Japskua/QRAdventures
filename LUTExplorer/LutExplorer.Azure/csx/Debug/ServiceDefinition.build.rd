<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="LutExplorer.Azure" generation="1" functional="0" release="0" Id="cb0b4bc7-8ff1-4dbf-a327-cbbd507813d5" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
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
          <role name="LutExplorer" generation="1" functional="0" release="0" software="C:\Projects\QRAdventures\LUTExplorer\LutExplorer.Azure\csx\Debug\roles\LutExplorer" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
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
    <implementation Id="fa16b3ee-b3ed-49f9-8b09-0f5c1a5694f4" ref="Microsoft.RedDog.Contract\ServiceContract\LutExplorer.AzureContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="6cdefba2-f0f4-4193-92e1-9bd4e8d83d69" ref="Microsoft.RedDog.Contract\Interface\LutExplorer:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/LutExplorer.Azure/LutExplorer.AzureGroup/LutExplorer:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>