//商品飞到购物篮中
function flyToCart(goodsId) 
{
	obj = $(goodsId);
	var target_top = $("#mini_basket").offset().top;
	var target_left = $("#mini_basket").offset().left;
	var clone_obj = obj.find(".thumb");
	var position = clone_obj.position();
	var width = clone_obj.width();
	var height = clone_obj.height();
	var new_clone = clone_obj.clone().insertAfter("footer"); //复制一个到飞行容器中
	//new_clone.;
	new_clone.css({ position: "absolute", "z-index": "99999", top: target_top - 200, left: target_left - 60, width: width, height: height, border: "1px solid #2d96b1" }).animate(
	{ top: target_top, left: target_left, width: "20px", height: "20px" }, //飞到购物篮图标上方
	400,
	function () {
		new_clone.fadeOut().remove(); //消失并清空内容
	}
	);
} 

//加入购物车
function addToCart(id) 
{
    $.ajax({
        type: 'post', cache: false, dataType: 'json',
        url: 'buy.php',
        data: [
                                            { name: 'controller', value: 'Ecs_cartManager' },
                                            { name: 'act', value: 'ajax_add_to_cart' },
                                            { name: 'goods_id', value: id }
                                            ],
        success: function (result) {
            if (result.success) {
                var count = parseInt($("#mini_basket").html()) + 1;
                $("#mini_basket").html(count);
            }
            else {
                alert(result.msg);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("出错了," + errorThrown);
        }
    });
}