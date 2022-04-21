<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Stagwell.Horiondigital.Modules.$safeprojectname$.View" %>
<script src="https://unpkg.com/react@16.4.1/umd/react.production.min.js"></script>
<script src="https://unpkg.com/react-dom@16.4.1/umd/react-dom.production.min.js"></script>
<div class="image-test"></div>
<img src="<%= ModulePath %>Images/authentication.png" />
<span><%= DoSomething(ModulePath) %></span>
<!-- #include file="../ReactTestApp/index_bbbbb.html" -->