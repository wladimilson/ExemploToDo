// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function() {
    var todoListItem = $('.todo-list');
    var todoListInput = $('.todo-list-input');

    const update = () => {
        todoListItem.html("");
        $.ajax({
            type : "GET",
            url: '/api/todo',
            contentType: "application/json",
            success: function(data) {
                var html = '';
                data.forEach((item) => {
                    html += (item.isComplete ? '<li class="completed">' : '<li>') 
                         +  "<div class='form-check'>"
                         +  "<label class='form-check-label'>"
                         +  "<input class='checkbox' type='checkbox' "
                         +  "value='" + item.id + "' "
                         +  (item.isComplete ? 'checked="checked"' : '') 
                         + " />" 
                         +    item.name 
                         +  "<i class='input-helper'></i>"
                         +  "</label>"
                         +  "</div>"
                         +  "<i class='remove mdi mdi-close-circle-outline' data-id='" + item.id + "'></i>"
                         +  "</li>";
                });
                todoListItem.append(html);
            },
            error: function(error) {
                console.log(error);
            }
        });
    };

    $('.todo-list-add-btn').on("click", function(event) {
        event.preventDefault();

        var item = $(this).prevAll('.todo-list-input').val();
        todoListInput.val("");

        if (item) {
            var data = {
                Name: item
            };

            $.ajax({
                type: "POST",
                url: '/api/todo',
                data: JSON.stringify({ "Name": item }),
                contentType: "application/json",
                success: function( data ) {
                    update();
                },
                error: function(error) {
                    console.log(error);
                }
            });
        }
    });

    todoListItem.on('change', '.checkbox', function() {
        if ($(this).attr('checked')) {
            $(this).removeAttr('checked');
        } else {
            $(this).attr('checked', 'checked');
        }

        $(this).closest("li").toggleClass('completed');
        console.log($(this).val());
        $.ajax({
            type: "PUT",
            url: '/api/todo/' + $(this).val(),
            data: JSON.stringify({ "isComplete": true }),
            contentType: "application/json",
            success: function( data ) {
                console.log(data);
            },
            error: function(error) {
                console.log(error);
            }
        });
    });

    todoListItem.on('click', '.remove', function() {
        $.ajax({
            type: "DELETE",
            url: '/api/todo/' + $(this).data('id'),
            data: JSON.stringify({ "isComplete": true }),
            contentType: "application/json",
            success: function( data ) {
                update();
            },
            error: function(error) {
                console.log(error);
            }
        });
    });
});