﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('#togglePassword').click(function () {
        var passwordField = $('#password');
        var fieldType = passwordField.attr('type');
        if (fieldType === 'password') {
            passwordField.attr('type', 'text');
            $(this).find('i').removeClass('fa-eye-slash').addClass('fa-eye');
        } else {
            passwordField.attr('type', 'password');
            $(this).find('i').removeClass('fa-eye').addClass('fa-eye-slash');
        }
    });

    $('#toggleConfirmPassword').click(function () {
        var confirmPasswordField = $('#confirmPassword');
        var fieldType = confirmPasswordField.attr('type');
        if (fieldType === 'password') {
            confirmPasswordField.attr('type', 'text');
            $(this).find('i').removeClass('fa-eye-slash').addClass('fa-eye');
        } else {
            confirmPasswordField.attr('type', 'password');
            $(this).find('i').removeClass('fa-eye').addClass('fa-eye-slash');
        }
    });
});