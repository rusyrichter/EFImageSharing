$(() => {
    const id = $("#image-id").val();

    setInterval(() => {
        $.get('/home/getLikes', { id }, function (likes) {
            $("#likes-count").text(likes);
        })
    })

    $(".btn-lg").on('click', function () {
       
        $.post('/home/addlike', { id }, function () { 
        })
    })
})