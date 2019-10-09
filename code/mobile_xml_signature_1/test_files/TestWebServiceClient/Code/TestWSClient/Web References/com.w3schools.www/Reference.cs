﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.42.
// 
#pragma warning disable 1591

namespace TestWSClient.com.w3schools.www {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="TempConvertSoap", Namespace="http://tempuri.org/")]
    public partial class TempConvert : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback FahrenheitToCelsiusOperationCompleted;
        
        private System.Threading.SendOrPostCallback CelsiusToFahrenheitOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public TempConvert() {
            this.Url = global::TestWSClient.Properties.Settings.Default.TestWSClient_com_w3schools_www_TempConvert;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event FahrenheitToCelsiusCompletedEventHandler FahrenheitToCelsiusCompleted;
        
        /// <remarks/>
        public event CelsiusToFahrenheitCompletedEventHandler CelsiusToFahrenheitCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/FahrenheitToCelsius", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public short FahrenheitToCelsius(short Fahrenheit) {
            object[] results = this.Invoke("FahrenheitToCelsius", new object[] {
                        Fahrenheit});
            return ((short)(results[0]));
        }
        
        /// <remarks/>
        public void FahrenheitToCelsiusAsync(short Fahrenheit) {
            this.FahrenheitToCelsiusAsync(Fahrenheit, null);
        }
        
        /// <remarks/>
        public void FahrenheitToCelsiusAsync(short Fahrenheit, object userState) {
            if ((this.FahrenheitToCelsiusOperationCompleted == null)) {
                this.FahrenheitToCelsiusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnFahrenheitToCelsiusOperationCompleted);
            }
            this.InvokeAsync("FahrenheitToCelsius", new object[] {
                        Fahrenheit}, this.FahrenheitToCelsiusOperationCompleted, userState);
        }
        
        private void OnFahrenheitToCelsiusOperationCompleted(object arg) {
            if ((this.FahrenheitToCelsiusCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.FahrenheitToCelsiusCompleted(this, new FahrenheitToCelsiusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/CelsiusToFahrenheit", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public short CelsiusToFahrenheit(short Celsius) {
            object[] results = this.Invoke("CelsiusToFahrenheit", new object[] {
                        Celsius});
            return ((short)(results[0]));
        }
        
        /// <remarks/>
        public void CelsiusToFahrenheitAsync(short Celsius) {
            this.CelsiusToFahrenheitAsync(Celsius, null);
        }
        
        /// <remarks/>
        public void CelsiusToFahrenheitAsync(short Celsius, object userState) {
            if ((this.CelsiusToFahrenheitOperationCompleted == null)) {
                this.CelsiusToFahrenheitOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCelsiusToFahrenheitOperationCompleted);
            }
            this.InvokeAsync("CelsiusToFahrenheit", new object[] {
                        Celsius}, this.CelsiusToFahrenheitOperationCompleted, userState);
        }
        
        private void OnCelsiusToFahrenheitOperationCompleted(object arg) {
            if ((this.CelsiusToFahrenheitCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CelsiusToFahrenheitCompleted(this, new CelsiusToFahrenheitCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.42")]
    public delegate void FahrenheitToCelsiusCompletedEventHandler(object sender, FahrenheitToCelsiusCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class FahrenheitToCelsiusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal FahrenheitToCelsiusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public short Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((short)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.42")]
    public delegate void CelsiusToFahrenheitCompletedEventHandler(object sender, CelsiusToFahrenheitCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CelsiusToFahrenheitCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CelsiusToFahrenheitCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public short Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((short)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591