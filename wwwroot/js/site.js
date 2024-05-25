// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
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

$(document).ready(function () {
    $('#reply-button').on('click', function () {
        var replyModal = new bootstrap.Modal(document.getElementById('replyModal'));
        replyModal.show();
    });

    $('#message-form').on('submit', function (e) {
        e.preventDefault();
        var form = $(this);
        var actionUrl = '/Theme/AddMessage';
        $.ajax({
            type: 'POST',
            url: actionUrl,
            data: form.serialize(),
            success: function (response) {
                if (response.success) {
                    var newMessageHtml = '<tr>' +
                        '<td>' + response.message.userNickName + '</td>' +
                        '<td>' + response.message.text + '</td>' +
                        '<td>' + response.message.dateOfSend + '</td>' +
                        '</tr>';
                    $('#messages').append(newMessageHtml);
                    $('#text').val('');
                    $('#replyModal').modal('hide');
                    $('html, body').animate({ scrollTop: $(document).height() }, 'slow');
                } else {
                    $('#error-message').text('Произошла ошибка при добавлении комментария.');
                }
            },
            error: function (response) {
                console.error("Error response:", response);
                var errorMessage = "Произошла ошибка при добавлении комментария.";
                if (response.responseText) {
                    errorMessage += " " + response.responseText;
                }
                $('#error-message').text(errorMessage);
            }
        });
    });
});
